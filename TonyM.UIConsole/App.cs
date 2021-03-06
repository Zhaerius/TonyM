using Microsoft.Extensions.Options;
using TonyM.Core.Interfaces;
using TonyM.Core.Models.Opts;
using TonyM.UIConsole.Helpers;

namespace TonyM.UIConsole
{
    public class App
    {
        private readonly IProductService productService;
        private readonly ISearchStatutService searchService;
        private readonly UserOptions userOptions;

        public App(IProductService productService, ISearchStatutService searchService, IOptions<UserOptions> userOptions)
        {
            this.productService = productService;
            this.searchService = searchService;
            this.userOptions = userOptions.Value;
        }

        public async Task RunAsync()
        {
            this.searchService.IsSearching = true;

            if (this.searchService.IsSearching)
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

                while (this.searchService.IsSearching)
                {
                    try
                    {
                        await productService.SearchStockAsync(products);
                        await Task.Delay(1000);
                    }
                    catch (Exception e)
                    {
                        UiHelpers.TextColor(e.Message, ConsoleColor.Red);
                        Console.WriteLine("Nouvelle tentative...");
                    }
                }
            }        
        }


    }
}