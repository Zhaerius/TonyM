using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TonyM.APP;
using TonyM.BLL.Services;
using TonyM.DAL.Repository;
using TonyM.DAL.Services;



var services = new ServiceCollection();
ConfigureServices(services);

using var serviceProvider = services.BuildServiceProvider();

await serviceProvider.GetService<App>().Run();


static void ConfigureServices(IServiceCollection services)
{
    //Core DI for TonyM
    var configuration = new ConfigurationBuilder()
        .AddJsonFile("UserSettings.json")
        .Build();

    services.AddSingleton<IConfiguration>(configuration);
    services.AddSingleton<App>();
    services.AddTransient<IRepository, Repository>();
    services.AddTransient<IBusiness, Business>();
    services.AddHttpClient<NvidiaHttpService>();
}