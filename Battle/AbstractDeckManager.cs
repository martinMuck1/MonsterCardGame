using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonsterCardGame.Cards;

namespace MonsterCardGame.Battle
{
    public class AbstractDeckManager
    {
        protected List<AbstractCard> _cardCollection = new List<AbstractCard>();   

        public AbstractDeckManager()
        {
        }

        //Factory for Cards => input is from db (cardModels)
        public static AbstractCard GetAbstractCard(string cardname, int strength)
        {
            AbstractCard tmpCard = null;
            switch (cardname)
            {
                case "Dragon": 
                    tmpCard = new FireDragon(strength);
                    break;
                case "FireElf":
                    tmpCard = new FireElves(strength);
                    break;
                case "Troll":
                    tmpCard = new FireTroll(strength);
                    break;
                case "Knight":
                    tmpCard = new NormalKnight(strength);
                    break;
                case "Ork":
                    tmpCard = new NormalOrk(strength);
                    break;
                case "Wizzard":
                    tmpCard = new NormalWizzard(strength);
                    break;
                case "WaterGoblin":
                    tmpCard = new WaterGoblin(strength);
                    break;
                case "Kraken":
                    tmpCard = new WaterKraken(strength);
                    break;
                case "FireSpell":
                    tmpCard = new FireSpell(strength);
                    break;
                case "RegularSpell":
                    tmpCard = new NormalSpell(strength);
                    break;
                case "WaterSpell":
                    tmpCard = new WaterSpell(strength);
                    break;
                default:
                    break;
            }
            return tmpCard;
        }

        public void AddCard(AbstractCard card)
        {
            this._cardCollection.Add(card);
        }

        public void RemoveCard(int index)
        {
            this._cardCollection.RemoveAt(index);
        }
         public AbstractCard showCard(int index)
        {
            return this._cardCollection[index];
        }
        public int getSizeStack()
        {
            return this._cardCollection.Count();
        }

        public void showAllCards()
        {
            foreach (var item in this._cardCollection)
            {
                Console.WriteLine("{0} : {1}",item.Name,item.Damage);
            }
        }
    }
}
