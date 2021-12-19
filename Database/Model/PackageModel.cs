using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCardGame.Database
{
    public class PackageModel
    {
        public string PackageID { get; private set; }
        public List<CardModel> PackageCards { get; private set; }

        public PackageModel(string packageID)
        {
            this.PackageID = packageID;
            this.PackageCards = new List<CardModel>();
        }

        public void AddCard(string id, string name, float damage)
        {
            this.PackageCards.Add(new CardModel(id, name, damage));
        }
    }
}
