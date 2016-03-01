using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace StudyBox.Services
{
    public static class RestService
    {
        private static ResourceDictionary _resources = App.Current.Resources;

        public static async Task<string> DownloadFlashcardsJson()
        {
            string url = (String)_resources["FlashcardsGetUrl"];
            string webPageSource = await GetWebPageSource(url, null);
            return webPageSource;
        }

        public static async Task<string> DownloadFlashcardsJson(CancellationTokenSource cts)
        {
            string url = (String)_resources["FlashcardsGetUrl"];
            string webPageSource = await GetWebPageSource(url, cts);
            return webPageSource;
        }

        public static async Task<string> DownloadFlashcardSetsJson()
        {
            string url = (String)_resources["FlashcardSetsGetUrl"];
            string webPageSource = await GetWebPageSource(url, null);
            return webPageSource;
        }

        public static async Task<string> DownloadFlashcardSetsJson(CancellationTokenSource cts)
        {
            string url = (String)_resources["FlashcardSetsGetUrl"];
            string webPageSource = await GetWebPageSource(url, cts);
            return webPageSource;
        }

        private static async Task<string> GetWebPageSource(string url, CancellationTokenSource cts)
        {
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response;
            string jsonString = "";
            try
            {
                if (cts == null)
                {
                    response = await httpClient.GetAsync(url);
                }
                else
                {
                    response = await httpClient.GetAsync(url, cts.Token);
                }

                var byteContent = await response.Content.ReadAsByteArrayAsync();

                try
                {
                    string encoding = response.Content.Headers.ContentType.CharSet;
                    jsonString = Encoding.GetEncoding(encoding).GetString(byteContent);
                }
                catch
                {
                    jsonString = Encoding.UTF8.GetString(byteContent);
                }

                return jsonString;
            }
            catch (TaskCanceledException ex)
            {
                throw (ex);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
