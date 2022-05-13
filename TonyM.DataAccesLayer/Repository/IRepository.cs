using TonyM.DAL.Models;

namespace TonyM.DAL.Repository
{
    public interface IRepository
    {
        Task<ListMap> GetProductFromSource(string reference, string locale);
    }
}