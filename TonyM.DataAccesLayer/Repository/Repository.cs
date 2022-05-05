using System.Text.Json;
using Microsoft.Extensions.Configuration;
using TonyM.DAL.Exceptions;
using TonyM.DAL.Models;

namespace TonyM.DAL.Repository
{
    public class Repository : IRepository
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _locale;

        public Repository(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            this._configuration = configuration;
            this._httpClientFactory = httpClientFactory;
            this._locale = configuration.GetSection("Locale").Value;
        }

        public async Task<ListMap> GetProductFromSource(string reference)
        {
            double timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();

            var httpClient = this._httpClientFactory.CreateClient("NvidiaClient");
            var httpResponseMessage = await httpClient.GetAsync($"feinventory?skus={reference}&locale={_locale}&timestamp={timestamp}");

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
            string localisation = _locale;
            var referencesList = _configuration.GetSection("Gpu").Value.Split(",");

            var products = new List<ListMap>();

            foreach (var reference in referencesList)
            {
                var product = new ListMap() { fe_sku = reference, locale = localisation };
                products.Add(product);
            }

            return products;
        }

    }
}