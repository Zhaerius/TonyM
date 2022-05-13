using System.Text.Json;
using Microsoft.Extensions.Configuration;
using TonyM.DAL.Exceptions;
using TonyM.DAL.Models;

namespace TonyM.DAL.Repository
{
    public class Repository : IRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public Repository(IHttpClientFactory httpClientFactory)
        {
            this._httpClientFactory = httpClientFactory;
        }

        public async Task<ListMap> GetProductFromSource(string reference, string locale)
        {
            double timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();

            var httpClient = this._httpClientFactory.CreateClient("NvidiaClient");
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

    }
}




