using System.Net.Http;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using TonyM.DAL.Exceptions;

namespace TonyM.DAL
{
    public class Repository : IRepository
    {
        private readonly IConfiguration configuration;
        private readonly IHttpClientFactory httpClientFactory;

        public Repository(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            this.configuration = configuration;
            this.httpClientFactory = httpClientFactory;            
        }


        public async Task<ListMap> GetProductFromSource(string reference, string locale)
        {
            double timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();

            var httpClient = this.httpClientFactory.CreateClient("NvidiaClient");
            var httpResponseMessage = await httpClient.GetAsync($"feinventory?skus={reference}&locale={locale}&timestamp={timestamp}");

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                using (var content = await httpResponseMessage.Content.ReadAsStreamAsync())
                {
                    try
                    {
                        var root = JsonSerializer.Deserialize<Root>(content);
                        return root.listMap.First();
                    }
                    catch (Exception)
                    {
                        throw new DeserializeException(reference);
                    }
                }
            }
            else
            {
                throw new HttpResponseException(reference);
            }
        }


        public IEnumerable<ListMap> GetProductFromConfig()
        {
            string localisation = configuration.GetSection("Locale").Value;
            var referencesList = configuration.GetSection("Gpu").Value.Split(",");

            var products = new List<ListMap>();

            foreach (var reference in referencesList)
            {
                var product = new ListMap() { fe_sku = reference, locale = localisation};
                products.Add(product);
            }

            return products;
        }

    }
}
