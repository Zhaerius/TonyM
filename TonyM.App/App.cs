using TonyM.APP.Helpers;
using TonyM.BLL.Services;

namespace TonyM.APP
{
    public class App
    {
        private readonly IBusiness _business;

        public App(IBusiness business)
        {
            _business = business;
        }

        public async Task Run()
        {
            var products = _business.Initialisation();

            foreach (var p in products)
                p.OnAvailable += UiHelpers.Alert;

            #region Affichage lancement
            Console.WriteLine(UiHelpers.Logo);
            Console.WriteLine("Appuyer sur une touche pour commencer la recherche");
            Console.ReadKey();
            Console.WriteLine("\n== VOTRE SELECTION ==");
            Console.WriteLine(string.Join(", ", products.Select(p => p.Name)));
            #endregion

            while (products.Count() > 0)
            {

                #region Affichage des derniers drops
                var productsDetected = products.Where(p => p.LastDetected != null).AsEnumerable();
                int nbProductsDetected = productsDetected.Count();

                if (nbProductsDetected > 0)
                {
                    Console.WriteLine("== DERNIERS DROPS ==");

                    foreach (var product in productsDetected)
                        Console.WriteLine($"{product.Name} : {product.LastDetected}");
                }
                #endregion

                #region Mise à jour des produit chez Nvidia et Detection
                CancellationTokenSource tokenSource = new CancellationTokenSource();
                CancellationToken token = tokenSource.Token;

                Console.Write("\nRecherche en cours");

                var displayProgress = Task.Run(() => UiHelpers.DisplayProgress(token));
                var process = products.Select(async p =>
                {
                    string? oldLink = p.BuyLink;
                    await _business.UpdateFromSourceAsync(p);
                    p.VerificationStock(oldLink);
                });

                try
                {
                    await Task.WhenAll(process);
                }
                catch (Exception)
                {
                    UiHelpers.ErrorTextColor("x");
                }
                #endregion

                #region Finalisation de l'affichage
                tokenSource.Cancel();
                await Task.Delay(1000);
                UiHelpers.ClearLastLine((nbProductsDetected == 0 ? 0 : 2) + nbProductsDetected);
                #endregion
            }

            Console.WriteLine("Merci d'avoir utiliser TonyM");
        }
    }
}
