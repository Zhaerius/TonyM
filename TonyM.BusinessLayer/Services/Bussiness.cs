using TonyM.DAL;

namespace TonyM.BLL
{
    public class ProductService : IBussiness
    {
        private readonly IRepository repository;

        public ProductService(IRepository repository)
        {
            this.repository = repository;
        }

        public IEnumerable<ProductBL> Initialisation()
        {
            var dao = repository.GetProductFromConfig();
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
            var dao = await repository.GetProductFromSource(product.Reference, product.Localisation);

            product.BuyLink = dao.product_url;
            product.InStock = bool.Parse(dao.is_active);
        }
    }
}
