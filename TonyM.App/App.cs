using Microsoft.Extensions.Options;
using TonyM.APP.Helpers;
using TonyM.BLL.Services;
using TonyM.Models.Opts;

namespace TonyM.APP
{
    public class App
    {
        private readonly IBusiness _business;
        private readonly UserOptions _userOptions;

        public App(IBusiness business, IOptions<UserOptions> userOptions)
        {
            _business = business;
            _userOptions = userOptions.Value;
        }

        public async Task RunAsync()
        {
            var products = _business.Initialisation(_userOptions.Gpus, _userOptions.Locale);

            foreach (var p in products)
                p.OnAvailable += UiHelpers.Alert;

            #region UI Lancement
            Console.WriteLine(UiHelpers.Logo);
            Console.WriteLine("Appuyer sur une touche pour commencer la recherche");
            Console.ReadKey();
            Console.WriteLine("\n== VOTRE SELECTION ==");
            Console.WriteLine(string.Join(", ", products.Select(p => p.Name)));
            #endregion

            while (products.Count() > 0)
            {
                #region UI derniers drops
                var productsDetected = products.Where(p => p.LastDetected != null).AsEnumerable();
                int nbProductsDetected = productsDetected.Count();

                if (nbProductsDetected > 0)
                {
                    Console.WriteLine("\n== DERNIERS DROPS ==");

                    foreach (var product in productsDetected)
                        Console.WriteLine($"{product.Name} : {product.LastDetected}");
                }
                #endregion

                #region UI Recherche
                CancellationTokenSource tokenSource = new CancellationTokenSource();
                CancellationToken token = tokenSource.Token;
                Console.Write("\nRecherche en cours");
                #endregion

                #region TonyM Core
                var displayProgress = Task.Run(() => UiHelpers.DisplayProgress(token));
                var process = products.Select(async p =>
                {
                    string? oldLink = p.BuyLink;
                    await _business.UpdateProductAsync(p);
                    p.VerificationStock(oldLink);
                });

                try
                {
                    await Task.WhenAll(process);
                }
                catch (Exception e)
                {
                    UiHelpers.TextColor(e.Message, ConsoleColor.Red);
                }
                #endregion                

                #region UI Fin
                tokenSource.Cancel();
                await Task.Delay(1000);
                UiHelpers.ClearLastLine((nbProductsDetected == 0 ? 1 : 3) + nbProductsDetected);
                #endregion
            }
        }
    }
}