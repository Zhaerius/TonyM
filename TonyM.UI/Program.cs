using Microsoft.Extensions.DependencyInjection;
using TonyM.APP;

var services = new ServiceCollection();
ConfigureServices(services);
TonyM.Infrastructure.Dependancies.ConfigureBasicServices(services);

using var serviceProvider = services.BuildServiceProvider();

await serviceProvider.GetService<Main>().MainAsync();


static void ConfigureServices(IServiceCollection services)
{
    services.AddSingleton<Main>();
}
