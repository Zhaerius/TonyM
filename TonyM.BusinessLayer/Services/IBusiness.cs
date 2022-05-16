using TonyM.BLL.Models;

namespace TonyM.BLL.Services
{
    public interface IBusiness
    {
        public IEnumerable<ProductBL> Initialisation(List<string> gpus, string locale);
        public Task UpdateProductAsync(ProductBL product);
    }
}
