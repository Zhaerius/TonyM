using System.Text.Json;
using TonyM.DAL.Exceptions;
using TonyM.DAL.Models;

namespace TonyM.DAL.Services
{
    public class NvidiaExternalService : INvidiaExternalService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly double timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();

        public NvidiaExternalService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ListMap> GetProductFromApiAsync(string reference, string locale)
        {
            var httpClient = _httpClientFactory.CreateClient("NvidiaClient");
            var httpResponseMessage = await httpClient.GetAsync($"feinventory?skus={reference}&locale={locale}&timestamp={timestamp}");

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                using (var content = await httpResponseMessage.Content.ReadAsStreamAsync())
                {
                    var root = JsonSerializer.Deserialize<Root>(content);

                    if (root.listMap.Count() == 0)
                        throw new DeserializeException(reference);

                    return root.listMap.First();                             
                }
            }
            throw new HttpResponseException(httpResponseMessage.StatusCode.ToString(), reference);         
        }

    }
}




