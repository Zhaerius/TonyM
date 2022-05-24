using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using TonyM.UIDiscord;
using TonyM.UIDiscord.Services;

var services = new ServiceCollection();
ConfigureServices(services);
TonyM.Infrastructure.Dependancies.ConfigureBasicServices(services);

using var serviceProvider = services.BuildServiceProvider();

await serviceProvider.GetService<App>().RunAsync();


static void ConfigureServices(IServiceCollection services)
{
    services.AddSingleton<App>();
    services.AddSingleton<DiscordSocketClient>();
    services.AddSingleton<LoggingService>();
    services.AddSingleton<CommandService>();
    services.AddSingleton<CommandHandlingService>();
}
