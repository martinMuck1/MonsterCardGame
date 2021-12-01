using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonsterCardGame.Cards;

namespace MonsterCardGame.DeckManage
{
    class AbstractDeckManager
    {
        public List<AbstractCard> cardCollection = new List<AbstractCard>();    //change to protected 
        public AbstractDeckManager()
        {
        }

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
        public void AddCard(AbstractCard card)
        {
            this.cardCollection.Add(card);
        }

        public void RemoveCard(int index)
        {
            this.cardCollection.RemoveAt(index);
        }
         public AbstractCard showCard(int index)
        {
            return this.cardCollection[index];
        }
        public int getSizeStack()
        {
            return this.cardCollection.Count();
        }

        public void showAllCards()
        {
            foreach (var item in this.cardCollection)
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
