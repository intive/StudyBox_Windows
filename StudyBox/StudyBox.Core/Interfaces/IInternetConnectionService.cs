using System.Threading.Tasks;

namespace StudyBox.Core.Interfaces
{
    public interface IInternetConnectionService
    {
        bool CheckConnection();
        Task<bool> IsNetworkAvailable();
        bool IsInternetAccess();
    }
}
