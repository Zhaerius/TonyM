using Microsoft.Extensions.DependencyInjection;
using TonyM.UIConsole;

var services = new ServiceCollection();
ConfigureServices(services);
TonyM.Infrastructure.Dependancies.ConfigureBasicServices(services);

using var serviceProvider = services.BuildServiceProvider();

await serviceProvider.GetService<App>().RunAsync();


static void ConfigureServices(IServiceCollection services)
{
    services.AddSingleton<App>();
}
