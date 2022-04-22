using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TonyM.BLL;
using TonyM.DAL;

namespace TonyM.IOC
{
    public static class Di
    {
        private static readonly IConfiguration Configuration;
        public static readonly IServiceProvider ServiceProvider;

        static Di()
        {
            // Configuration
            var configBuilder = new ConfigurationBuilder();
            configBuilder.AddJsonFile("AppSettings.json");
            configBuilder.AddJsonFile("UserSettings.json");
            Configuration = configBuilder.Build();



            // DI
            var services = new ServiceCollection();

            services.AddSingleton(Configuration);
            services.AddTransient<IRepository, Repository>();
            services.AddTransient<IBussiness, ProductService>();
            services.AddHttpClient("NvidiaClient", client =>
            {
                client.BaseAddress = new Uri(Configuration.GetSection("NvidiaApi").Value);
                client.Timeout = TimeSpan.FromSeconds(5);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("User-Agent", Configuration.GetSection("UserAgent").Value);
                client.DefaultRequestHeaders.Add("Cache-Control", "no-cache, no-store, must-revalidate");
                client.DefaultRequestHeaders.Add("Accept-Language", "fr-FR,fr;q=0.9,en-US;q=0.8,en;q=0.7");
                client.DefaultRequestHeaders.Add("Pragma", "no-cache");
            });


            ServiceProvider = services.BuildServiceProvider();
        }

    }
}