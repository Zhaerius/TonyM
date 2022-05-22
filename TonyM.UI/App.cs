using Microsoft.Extensions.Options;
using TonyM.APP.Helpers;
using TonyM.Core.Interfaces;
using TonyM.Core.Models.Opts;

namespace TonyM.APP
{
    public class App
    {
        private readonly IProductService productService;
        private readonly UserOptions userOptions;

        public App(IProductService productService, IOptions<UserOptions> userOptions)
        {
            this.productService = productService;
            this.userOptions = userOptions.Value;
        }

        public async Task RunAsync()
        {
            var products = productService.Initialisation(userOptions.Gpus, userOptions.Locale);

            foreach (var p in products)
                p.OnAvailable += UiHelpers.Alert;

            #region UI Lancement
            Console.WriteLine(UiHelpers.Logo);
            Console.WriteLine("\n== VOTRE SELECTION ==");
            Console.WriteLine(string.Join(", ", products.Select(p => p.Name)));
            Console.WriteLine("\nAppuyer sur une touche pour commencer la recherche");
            Console.ReadKey();
            Console.WriteLine("\nRecherche en cours...");
            #endregion

            try
            {
                while (true)
                {
                    await productService.SearchStockAsync(products);
                    await Task.Delay(1000);
                }              
            }
            catch (Exception e)
            {
                UiHelpers.TextColor(e.Message, ConsoleColor.Red);
            }

        }
    }
}