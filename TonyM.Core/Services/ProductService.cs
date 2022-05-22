using TonyM.Core.Interfaces;
using TonyM.Core.Models;

namespace TonyM.Core.Services
{
    public class ProductService : IProductService
    {
        private readonly INvidiaExternalService nvidiaExternalService;

        public ProductService(INvidiaExternalService nvidiaExternalService)
        {
            this.nvidiaExternalService = nvidiaExternalService;
        }

        public IEnumerable<Product> Initialisation(List<string> gpus, string locale)
        {
            var products = new List<Product>();

            foreach (var gpu in gpus)
            {
                var product = new Product(gpu, locale);
                products.Add(product);
            }

            return products;
        }

        public async Task SearchStockAsync(IEnumerable<Product> products)
        {
            var process = products.Select(async p =>
            {
                string? oldLink = p.BuyLink;

                var dao = await nvidiaExternalService.GetProductFromApiAsync(p.Reference, p.Localisation);
                p.BuyLink = dao.product_url;
                p.InStock = bool.Parse(dao.is_active);

                p.VerificationStock(oldLink);

            });

            await Task.WhenAll(process);
        }
    }
}
