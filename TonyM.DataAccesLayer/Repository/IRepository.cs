namespace TonyM.DAL
{
    public interface IRepository
    {
        Task<ListMap> GetProductFromSource(string reference, string locale);
        IEnumerable<ListMap> GetProductFromConfig();
    }
}