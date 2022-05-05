using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TonyM.APP;
using TonyM.APP.Services;
using TonyM.BLL.Services;
using TonyM.DAL.Repository;



var services = new ServiceCollection();
ConfigureServices(services);

using var serviceProvider = services.BuildServiceProvider();

await serviceProvider.GetService<App>().Run();


static void ConfigureServices(IServiceCollection services)
{
    var configuration = new ConfigurationBuilder()
        .AddJsonFile("UserSettings.json")
        .Build();

    //DI for TonyM Core
    services.AddSingleton<IConfiguration>(configuration);
    services.AddSingleton<App>();
    services.AddTransient<IRepository, Repository>();
    services.AddTransient<IBusiness, Business>();
    services.AddHttpClient("NvidiaClient", client =>
    {
        client.BaseAddress = new Uri("https://api.store.nvidia.com/partner/v1/");
        client.Timeout = TimeSpan.FromSeconds(5);
        client.DefaultRequestHeaders.Add("Accept", "application/json");
        client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/92.0.4515.107 Safari/537.36 OPR/77.0.4054.277");
        client.DefaultRequestHeaders.Add("Cache-Control", "no-cache, no-store, must-revalidate");
        client.DefaultRequestHeaders.Add("Accept-Language", "fr-FR,fr;q=0.9,en-US;q=0.8,en;q=0.7");
        client.DefaultRequestHeaders.Add("Pragma", "no-cache");
    });

    //DI for Discord
    services.AddSingleton<DiscordSocketClient>();
    services.AddSingleton<LoggingService>();
    services.AddSingleton<CommandService>();
    services.AddSingleton<CommandHandlingService>();
    services.AddSingleton<NvidiaStatutService>();
}