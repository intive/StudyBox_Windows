using StudyBox.Core.Interfaces;
using System;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;
using Windows.UI.Xaml;

namespace StudyBox.Core.Services
{
    public class GravatarService : IGravatarService
    {
        private readonly ResourceDictionary _resources = Application.Current.Resources;

        public string GetUserGravatarUrl(string email)
        {
            string hashedEmail = HashEmailForGravatar(email);
            return String.Format(_resources["GravatarUrl"].ToString(), hashedEmail);
        }

        public string GetDefaultGravatarUrl()
        {
            return String.Format(_resources["GravatarUrl"].ToString(), String.Empty);
        }

        private string HashEmailForGravatar(string email)
        {
            HashAlgorithmProvider algorithm = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Md5);
            IBuffer buffer = CryptographicBuffer.ConvertStringToBinary(email, BinaryStringEncoding.Utf8);
            IBuffer hashed = algorithm.HashData(buffer);
            return CryptographicBuffer.EncodeToHexString(hashed);
        }
    }
}