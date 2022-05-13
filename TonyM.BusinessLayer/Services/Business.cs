using Microsoft.Extensions.Options;
using TonyM.BLL.Models;
using TonyM.DAL.Repository;
using TonyM.Models.Opts;

namespace TonyM.BLL.Services
{
    public class Business : IBusiness
    {
        private readonly IRepository repository;
        private readonly UserOptions userOptions;

        public Business(IRepository repository, IOptions<UserOptions> userOptions)
        {
            this.repository = repository;
            this.userOptions = userOptions.Value;
        }

        public IEnumerable<ProductBL> Initialisation()
        {
            var userSearchList = userOptions.Gpus.Split(",");
            var products = new List<ProductBL>();

            foreach (var p in userSearchList)
            {
                var product = new ProductBL(p, userOptions.Locale);
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
