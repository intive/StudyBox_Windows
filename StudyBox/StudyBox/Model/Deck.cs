using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyBox.Model
{
    public class Deck
    {
        public string ID { get; private set; }
        public string Name { get; private set; }

        public Deck() { }

        public Deck(string id)
        {
            this.ID = id;
        }

        public Deck(string id, string name)
        {
            this.ID = id;
            this.Name = name;
        }
    }
}
