using TonyM.DAL.Models;

namespace TonyM.DAL.Services
{
    public interface INvidiaService
    {
        Task<ListMap> GetProductFromApiAsync(string reference, string locale);
    }
}