using TonyM.Core.Models;

namespace TonyM.Core.Interfaces
{
    public interface IProductService
    {
        public IEnumerable<Product> Initialisation(List<string> gpus, string locale);
        public Task SearchStockAsync(IEnumerable<Product> products);
    }
}
