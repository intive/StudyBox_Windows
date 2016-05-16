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
            InitializeFile();
            try
            {
                string path = ApplicationData.Current.LocalFolder.Path + "/" + UserId + _resources["FavouriteDecksFileName"].ToString();

                List<string> decksToSave = new List<string>();
                foreach (Deck deck in favouriteDekcs)
                    decksToSave.Add(String.Format("{0};{1}", deck.ID, deck.ViewModel.AddToFavouriteDecksDate));

                File.WriteAllLines(path, decksToSave);
            }
            catch (Exception)
            {
            }
        }

        public List<Deck> GetFavouriteDecks()
        {
            InitializeFile();
            var favouriteDecks = new List<Deck>();
            try
            {
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
