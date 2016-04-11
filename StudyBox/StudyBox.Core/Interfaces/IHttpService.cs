using System.Threading;
using System.Threading.Tasks;
using StudyBox.Core.Models;

namespace StudyBox.Core.Interfaces
{
    public interface IHttpService
    {
        Task<int> Login(User user, CancellationTokenSource cts = null);
        void SetAuthenticationHeader(string email, string password);
    }
}