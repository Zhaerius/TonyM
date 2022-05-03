using TonyM.BLL.Models;
using TonyM.DAL.Repository;

namespace TonyM.BLL.Services
{
    public class Business : IBusiness
    {
        private readonly IRepository _repository;

        public Business(IRepository repository)
        {
            this._repository = repository;
        }

        public IEnumerable<ProductBL> Initialisation()
        {
            var dao = _repository.GetProductFromConfig();
            var products = new List<ProductBL>();

            foreach (var d in dao)
            {
                var product = new ProductBL(d.fe_sku, d.locale);
                products.Add(product);
            }
            return products;
        }

        public async Task UpdateFromSourceAsync(ProductBL product)
        {
            var dao = await _repository.GetProductFromSource(product.Reference);

            product.BuyLink = dao.product_url;
            product.InStock = bool.Parse(dao.is_active);
        }
    }
}
