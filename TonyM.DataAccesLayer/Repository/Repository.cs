using System.Text.Json;
using Microsoft.Extensions.Configuration;
using TonyM.DAL.Exceptions;
using TonyM.DAL.Models;
using TonyM.DAL.Services;

namespace TonyM.DAL.Repository
{
    public class Repository : IRepository
    {
        private readonly IConfiguration _configuration;
        private readonly NvidiaHttpService _client;

        public Repository(IConfiguration configuration, NvidiaHttpService nvidiaHttpService)
        {
            this._configuration = configuration;
            this._client = nvidiaHttpService;            
        }

        public async Task<ListMap> GetProductFromSource(string reference)
        {
            var httpResponseMessage = await _client.GetNvidiaResponse(reference);

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
            string localisation = _configuration.GetSection("Locale").Value;
            var referencesList = _configuration.GetSection("Gpu").Value.Split(",");

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
