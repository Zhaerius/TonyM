using Microsoft.Extensions.Configuration;

namespace TonyM.DAL.Services
{
    public class NvidiaHttpService
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _configuration;
        private readonly double _timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();

        public NvidiaHttpService(HttpClient httpClient, IConfiguration configuration)
        {
            _client = httpClient;
            _configuration = configuration;

            _client.BaseAddress = new Uri(_configuration.GetSection("NvidiaApi").Value);
            _client.Timeout = TimeSpan.FromSeconds(5);
            _client.DefaultRequestHeaders.Add("Accept", "application/json");
            _client.DefaultRequestHeaders.Add("User-Agent", _configuration.GetSection("UserAgent").Value);
            _client.DefaultRequestHeaders.Add("Cache-Control", "no-cache, no-store, must-revalidate");
            _client.DefaultRequestHeaders.Add("Accept-Language", "fr-FR,fr;q=0.9,en-US;q=0.8,en;q=0.7");
            _client.DefaultRequestHeaders.Add("Pragma", "no-cache");
        }

        public async Task<HttpResponseMessage> GetNvidiaResponse(string reference)
        {
            return await _client.GetAsync($"feinventory?skus={reference}&locale={_configuration.GetSection("Locale").Value}&timestamp={_timestamp}");
        }
    }
}
