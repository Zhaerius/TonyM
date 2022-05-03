using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TonyM.APP;
using TonyM.APP.Services;
using TonyM.BLL.Services;
using TonyM.DAL.Repository;
using TonyM.DAL.Services;



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
    services.AddHttpClient<NvidiaHttpService>();

    //DI for Discord
    services.AddSingleton<DiscordSocketClient>();
    services.AddSingleton<LoggingService>();
    services.AddSingleton<CommandService>();
    services.AddSingleton<CommandHandlingService>();
    services.AddSingleton<NvidiaStatutService>();
}