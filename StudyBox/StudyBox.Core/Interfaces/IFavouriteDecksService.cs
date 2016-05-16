using StudyBox.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyBox.Core.Interfaces
{
    public interface IFavouriteDecksService
    {
        void InitializeFile();
        void SaveFavouriteDecks(List<Deck> favouriteDekcs);
        List<Deck> GetFavouriteDecks();
    }
}
