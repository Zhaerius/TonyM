using Microsoft.Extensions.DependencyInjection;
using TonyM.APP;

var services = new ServiceCollection();
TonyM.Infrastructure.Dependancies.ConfigureBasicServices(services);

using var serviceProvider = services.BuildServiceProvider();

await serviceProvider.GetService<App>().RunAsync();


static void ConfigureServices(IServiceCollection services)
{
    services.AddSingleton<App>();
}

    //.AddPolicyHandler(PolicyRetry.GetRetryPolicy());
