using Microsoft.Extensions.DependencyInjection;
using TonyM.BLL;
using TonyM.IOC;
using TonyM.UI;

string logo = @"
  _____                 __  __         ____  
 |_   _|__  _ __  _   _|  \/  | __   _| ___| 
   | |/ _ \| '_ \| | | | |\/| | \ \ / /___ \ 
   | | (_) | | | | |_| | |  | |  \ V / ___) |
   |_|\___/|_| |_|\__, |_|  |_|   \_/ |____/ 
                  |___/                      
";

var business = Di.ServiceProvider.GetService<IBussiness>();
var products = business.Initialisation();

foreach (var p in products)
    p.OnAvailable += UiHelpers.Alert;

Console.WriteLine(logo);
Console.WriteLine("Appuyer sur une touche pour commencer la recherche");
Console.ReadKey();
Console.WriteLine("\n== VOTRE SELECTION ==");
Console.WriteLine(string.Join(", ", products.Select(p => p.Name)));
Console.WriteLine();

while (products.Count() > 0)
{

    #region Affichage des derniers drops
    var productsDetected = products.Where(p => p.LastDetected != null).AsEnumerable();
    int nbProductsDetected = productsDetected.Count();

    if (nbProductsDetected > 0)
    {
        Console.WriteLine("== DERNIERS DROPS ==");

        foreach (var product in productsDetected)
            Console.WriteLine($"{product.Name} : {product.LastDetected} \n" );
    }
    #endregion

    #region Mise à jour des produit chez Nvidia et Detection
    CancellationTokenSource tokenSource = new CancellationTokenSource();
    CancellationToken token = tokenSource.Token;

    Console.Write("Recherche en cours");

    var displayProgress = Task.Run(() => UiHelpers.DisplayProgress(token));
    var process = products.Select(async p => 
    {
        string? oldLink = p.BuyLink;
        await business.UpdateFromSourceAsync(p);
        p.VerificationStock(oldLink);
    });

    try
    {
        await Task.WhenAll(process);
    }
    catch (Exception e)
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







