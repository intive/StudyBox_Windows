using StudyBox.Core.Interfaces;
using StudyBox.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;

namespace StudyBox.Core.Services
{
    public class FavouriteDecksService : IFavouriteDecksService
    {
        public string UserId { get; set; }
        private readonly ResourceDictionary _resources = Application.Current.Resources;
        private IUserDataStorageService _userDataStorageService;

        public FavouriteDecksService(IUserDataStorageService userDataStorageService)
        {
            UserId = String.Empty;
            _userDataStorageService = userDataStorageService;
            InitializeFile();
        }

        public void InitializeFile()
        {
            UserId = _userDataStorageService.GetUserEmail();
            if (UserId == null)
                UserId = String.Empty;
            string path = ApplicationData.Current.LocalFolder.Path + "/" + UserId + _resources["FavouriteDecksFileName"].ToString();
            if (!File.Exists(path))
                File.Create(path);
        }

        public void SaveFavouriteDecks(List<Deck> favouriteDekcs)
        {
            
            try
            {
                UserId = _userDataStorageService.GetUserEmail();
                if (UserId == null)
                    UserId = String.Empty;

                string path = ApplicationData.Current.LocalFolder.Path + "/" + UserId + _resources["FavouriteDecksFileName"].ToString();

                if (!File.Exists(path))
                {
                    var createdFile = File.Create(path);
                    createdFile.Dispose();
                }

                string decksToSave = String.Empty;
                foreach (Deck deck in favouriteDekcs)
                    decksToSave+=String.Format("{0};{1}", deck.ID, deck.ViewModel.AddToFavouriteDecksDate)+"\n";

                using (var file = File.Open(path, FileMode.Open, FileAccess.Write, FileShare.Read))
                {
                    file.SetLength(0);
                    string fileContent = decksToSave;
                    byte[] toBytes = Encoding.ASCII.GetBytes(fileContent);
                    if (file.CanWrite)
                    {
                        file.Write(toBytes, 0, fileContent.Length);
                    }
                }
            }
            catch (Exception)
            {
                
            }
        }

        public List<Deck> GetFavouriteDecks()
        {
            var favouriteDecks = new List<Deck>();
            try
            {
                UserId = _userDataStorageService.GetUserEmail();
                if (UserId == null)
                    UserId = String.Empty;

                string path = ApplicationData.Current.LocalFolder.Path + "/" + UserId + _resources["FavouriteDecksFileName"].ToString();
                if (!File.Exists(path))
                    SaveFavouriteDecks(new List<Deck>());

                List<string> fileContent = File.ReadAllLines(path).ToList();

                foreach (string singleLine in fileContent)
                {
                    List<string> line = singleLine.Split(';').ToList();
                    Deck newDeck = new Deck(line[0]);
                    newDeck.ViewModel.IsFavourite = true;
                    newDeck.ID = line[0];
                    newDeck.ViewModel.AddToFavouriteDecksDate = Convert.ToDateTime(line[1]);
                    favouriteDecks.Add(newDeck);
                }
            }

            catch (Exception)
            {
                return null;
            }
            return favouriteDecks;
        }
    }
}
