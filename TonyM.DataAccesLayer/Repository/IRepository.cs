using TonyM.DAL.Models;

namespace TonyM.DAL.Repository
{
    public interface IRepository
    {
        Task<ListMap> GetProductFromApi(string reference, string locale);
    }
}