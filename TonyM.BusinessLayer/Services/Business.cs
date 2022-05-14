using TonyM.BLL.Models;
using TonyM.DAL.Repository;

namespace TonyM.BLL.Services
{
    public class Business : IBusiness
    {
        private readonly IRepository repository;

        public Business(IRepository repository)
        {
            this.repository = repository;
        }

        public IEnumerable<ProductBL> Initialisation(List<string> gpus, string locale)
        {
            var products = new List<ProductBL>();

            foreach (var gpu in gpus)
            {
                var product = new ProductBL(gpu, locale);
                products.Add(product);
            }

            return products;
        }

        public async Task UpdateProductAsync(ProductBL product)
        {
            var dao = await repository.GetProductFromApi(product.Reference, product.Localisation);

            product.BuyLink = dao.product_url;
            product.InStock = bool.Parse(dao.is_active);
        }
    }
}
