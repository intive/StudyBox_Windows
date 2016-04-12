using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using StudyBox.Core.Interfaces;
using StudyBox.Core.Models;

namespace StudyBox.Core.Services
{
    public class HttpService : IHttpService
    {
        private static readonly ResourceDictionary _resources = Application.Current.Resources;
        private readonly HttpClient _client = new HttpClient(); //{BaseAddress = new Uri(String.Format(_resources["UserGetLoggedUrl"].ToString()))}; 
        public async Task<int> Login(User user, CancellationTokenSource cts = null)
        {
            SetAuthenticationHeader(user.Email, user.Password);
            var response = await _client.GetAsync(String.Format(_resources["UserGetLoggedUrl"].ToString()));
            return (int)response.StatusCode;
        }

        public void SetAuthenticationHeader(string email, string password)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(String.Format("{0}:{1}", email, password))));
        }

        public void Logout()
        {
            _client.DefaultRequestHeaders.Authorization = null;
        }
    }
}
