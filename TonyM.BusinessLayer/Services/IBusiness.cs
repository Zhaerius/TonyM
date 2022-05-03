using TonyM.BLL.Models;

namespace TonyM.BLL.Services
{
    public interface IBusiness
    {
        public IEnumerable<ProductBL> Initialisation();
        public Task UpdateFromSourceAsync(ProductBL product);
    }
}
