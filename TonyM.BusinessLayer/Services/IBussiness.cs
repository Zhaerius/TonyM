namespace TonyM.BLL
{
    public interface IBussiness
    {
        public IEnumerable<ProductBL> Initialisation();
        public Task UpdateFromSourceAsync(ProductBL product);
    }
}
