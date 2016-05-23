using StudyBox.Core.Interfaces;
using StudyBox.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyBox.Core.Services
{
    public class TokenService : ITokenService
    {
        private readonly IInternetConnectionService _internetConnectionService;
        private readonly IRestService _restService;

        public TokenService(IInternetConnectionService internetConnectionService, IRestService restService)
        {
            _internetConnectionService = internetConnectionService;
            _restService = restService;
        }

        public Task<ResetPassword> GetToken(string email)
        {
            return _restService.ResetPassword(email);
        }
    }
}
