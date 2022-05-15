using System.Text.Json;
using Microsoft.Extensions.Configuration;
using TonyM.DAL.Exceptions;
using TonyM.DAL.Models;

namespace TonyM.DAL.Services
{
    public class NvidiaService : INvidiaService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly double timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();

        public NvidiaService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ListMap> GetProductFromApiAsync(string reference, string locale)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("NvidiaClient");
                var httpResponseMessage = await httpClient.GetAsync($"feinventory?skus={reference}&locale={locale}&timestamp={timestamp}");

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    using (var content = await httpResponseMessage.Content.ReadAsStreamAsync())
                    {
                        var root = JsonSerializer.Deserialize<Root>(content);
                        return root.listMap.First();
                    }
                }
                else
                {
                    throw new HttpRequestException();
                }
            }
            catch (Exception)
            {

                throw;
            }
            
        }

    }
}




