using StudyBox.Core.Interfaces;
using Windows.Networking.Connectivity;

namespace StudyBox.Core.Services
{
    public class InternetConnectionService : IInternetConnectionService
    {
        public bool CheckConnection()
        {
            ConnectionProfile internetConnectionProfile = NetworkInformation.GetInternetConnectionProfile();

            if (internetConnectionProfile == null)
            {
                return false;
            }

            return true;
        }
    }
}
