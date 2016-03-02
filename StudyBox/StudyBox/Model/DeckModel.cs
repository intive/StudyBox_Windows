using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyBox.Model
{
    public class DeckModel
    {
        public string ID { get; private set; }
        public string Name { get; private set; }

        public DeckModel() { }

        public DeckModel(string id)
        {
            this.ID = id;
        }

        public DeckModel(string id, string name)
        {
            this.ID = id;
            this.Name = name;
        }
    }
}
