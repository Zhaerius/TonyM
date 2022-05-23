using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TonyM.Core.Interfaces;
using TonyM.Core.Models.Opts;
using TonyM.Core.Services;
using TonyM.Infrastructure.Policies;
using TonyM.Infrastructure.Services;

namespace TonyM.Infrastructure
{
    public static class Dependancies
    {
        public static void ConfigureBasicServices(IServiceCollection services)
        {
            //DI for TonyM
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("UserSettings.json")
                .Build();

            services.AddOptions();
            services.Configure<UserOptions>(configuration.GetSection("UserResearch"));
            services.Configure<DiscordOptions>(configuration.GetSection("Discord"));

            services.AddSingleton<ISearchStatutService, SearchStatutService>();
            services.AddTransient<INvidiaExternalService, NvidiaExternalService>();
            services.AddTransient<IProductService, ProductService>();

            services.AddHttpClient("NvidiaClient", client =>
            {
                client.BaseAddress = new Uri("https://api.store.nvidia.com/partner/v1/");
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/92.0.4515.107 Safari/537.36 OPR/77.0.4054.277");
                client.DefaultRequestHeaders.Add("Cache-Control", "no-cache, no-store, must-revalidate");
                client.DefaultRequestHeaders.Add("Accept-Language", "fr-FR,fr;q=0.9,en-US;q=0.8,en;q=0.7");
                client.DefaultRequestHeaders.Add("Pragma", "no-cache");
            })
            .AddPolicyHandler(PolicyRetry.GetRetryPolicy());
        }
    }
}
