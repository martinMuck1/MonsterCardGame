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
        /*
        public AbstractCard CreateRandomCard()      //returns a random CardType with random strength
        {
            AbstractCard card = null;
            int strength = CreateRandomStrenght();
            Random rnd = new Random();
            int num = rnd.Next(11); //< 11 

            switch (num)
            {
                case 0:
                    card = new FireDragon(strength);
                    break;
                case 1:
                    card = new FireElves(strength);
                    break;
                case 2:
                    card = new FireTroll(strength);
                    break;
                case 3:
                    card = new NormalKnight(strength);
                    break;
                case 4:
                    card = new NormalOrk(strength);
                    break;
                case 5:
                    card = new NormalWizzard(strength);
                    break;
                case 6:
                    card = new WaterGoblin(strength);
                    break;
                case 7:
                    card = new WaterKraken(strength);
                    break;
                case 8:
                    card = new FireSpell(strength);
                    break;
                case 9:
                    card = new NormalSpell(strength);
                    break;
                case 10:
                    card = new WaterSpell(strength);
                    break;

                default:
                    card = null;
                    break;
            }

            return card;
        }
        protected int CreateRandomStrenght()        //maybe add frequences of strength
        {
            Random rnd = new Random();
            int num = rnd.Next(10,100); //>= 10 and <= 99
            return num;
        }
        */
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
        /*
        public AbstractCard FindCardInStack(AbstractCard card)
        {
            return new WaterGoblin(100);
        }
        */
    }
}
