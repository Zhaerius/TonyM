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
            #endregion

            #region UI Recherche
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            CancellationToken token = tokenSource.Token;
            Console.Write("\nRecherche en cours...");
            #endregion



            try
            {
                await productService.SearchStockAsync(products);
            }
            catch (Exception e)
            {
                UiHelpers.TextColor(e.Message, ConsoleColor.Red);
            }




            //#region UI derniers drops
            //var productsDetected = products.Where(p => p.LastDetected != null).AsEnumerable();
            //int nbProductsDetected = productsDetected.Count();

            //if (nbProductsDetected > 0)
            //{
            //    Console.WriteLine("\n== DERNIERS DROPS ==");

            //    foreach (var product in productsDetected)
            //        Console.WriteLine($"{product.Name} : {product.LastDetected}");
            //}
            //#endregion

            //#region UI Recherche
            //CancellationTokenSource tokenSource = new CancellationTokenSource();
            //CancellationToken token = tokenSource.Token;
            //Console.Write("\nRecherche en cours");
            //#endregion


            //#region UI Fin
            //tokenSource.Cancel();
            //await Task.Delay(1000);
            //UiHelpers.ClearLastLine((nbProductsDetected == 0 ? 1 : 3) + nbProductsDetected);
            //#endregion
        }
    }
}