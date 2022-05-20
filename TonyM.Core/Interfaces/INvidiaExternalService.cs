using TonyM.Core.Models;

namespace TonyM.Core.Interfaces
{
    public interface INvidiaExternalService
    {
        Task<ListMap> GetProductFromApiAsync(string reference, string locale);
    }
}