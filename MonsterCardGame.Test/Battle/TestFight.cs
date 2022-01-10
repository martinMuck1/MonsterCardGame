using NUnit.Framework;
using MonsterCardGame;
using MonsterCardGame.Cards;
using MonsterCardGame.Battle;
using System.Collections.Generic;
using Moq;


namespace MonsterCardGame.Test
{
    class TestFight
    {
        AbstractCard _card1, _card2;
        Fight _battle;
        Mock<Fight> _mockBattle;
        AbstractDeckManager _deckA;
        AbstractDeckManager _deckB;

        [SetUp]
        public void Setup()
        {
            _battle = new Fight(new Players("Martin"), new Players("Amila"));

            List<AbstractCard> listA = new List<AbstractCard>() { new WaterGoblin(10), new WaterGoblin(10), new WaterGoblin(10), new WaterGoblin(10) };
            List<AbstractCard> listB = new List<AbstractCard>() { new WaterGoblin(5), new WaterGoblin(5), new WaterGoblin(5), new WaterGoblin(5) };
            _deckA = new Deck(listA);
            _deckB = new Deck(listB);
            Players playerA = new Players("PlayerA");
            Players playerB = new Players("PlayerB");
            playerA.MyDeck = _deckA;
            playerB.MyDeck = _deckB;
            _mockBattle = new Mock<Fight>(playerA, playerB);
            _mockBattle.CallBase = true;
            _mockBattle.Setup(x => x.RoundFight(It.IsAny<AbstractCard>(), It.IsAny<AbstractCard>())).Returns(Outcome.winnerA);
        }

        //--------------Base Cases ---------------
        [TestCase(120, 100, Outcome.winnerA)]
        [TestCase(100, 120, Outcome.winnerB)]
        public void RoundFigth_MonstervsMonster_ShouldBeWinnerA_WinnerB(int strength1, int strength2, Outcome winner)
        {
            _card1 = new WaterGoblin(strength1);
            _card2 = new FireTroll(strength2);
            
            Assert.IsTrue(_battle.RoundFight(_card1, _card2) == winner);
        }

        [TestCase(10, 20, Outcome.winnerB)]
        [TestCase(20, 5, Outcome.deuce)]
        [TestCase(90, 5, Outcome.winnerA)]
        public void RoundFigth_SpellvsSpell_ShouldBeWinnerB_Draw_WinnerA(int strength1, int strength2, Outcome winner)
        {
            _card1 = new FireSpell(strength1);
            _card2 = new WaterSpell(strength2);

            Assert.IsTrue(_battle.RoundFight(_card1, _card2) == winner);
        }

        [Test]
        public void RoundFigth_SpellvsMonsterShouldBeWinnerB()
        {
            _card1 = new FireSpell(10);
            _card2 = new WaterGoblin(10);
            Assert.IsTrue(_battle.RoundFight(_card1, _card2) == Outcome.winnerB);
        }
        [Test]
        public void RoundFigth_SpellvsMonster_ShouldBeDraw()
        {
            _card1 = new WaterSpell(10);
            _card2 = new WaterGoblin(10);
            Assert.IsTrue(_battle.RoundFight(_card1, _card2) == Outcome.deuce);
        }
        [Test]
        public void RoundFigth_SpellvsMonster_ShouldBeWinnerA()
        {
            _card1 = new NormalSpell(10);
            _card2 = new WaterGoblin(10);
            Assert.IsTrue(_battle.RoundFight(_card1, _card2) == Outcome.winnerA);
        }
        [Test]
        public void RoundFigth_SpellvsMonster_ShouldBeWinnerB()
        {
            _card1 = new NormalSpell(10);
            _card2 = new NormalKnight(15);
            Assert.IsTrue(_battle.RoundFight(_card1, _card2) == Outcome.winnerB);
        }

        //--------------Special Cases ---------------
        [Test]
        public void RoundFigth_GoblinvsDragonShouldBeDragon()
        {
            _card1 = new WaterGoblin(10);
            _card2 = new FireDragon(10);
            Assert.IsTrue(_battle.RoundFight(_card1, _card2) == Outcome.winnerB);
        }
        [Test]
        public void RoundFigth_WizzardvsOrkShouldBeWizzard()
        {
            _card1 = new NormalWizzard(10);
            _card2 = new NormalOrk(10);
            Assert.IsTrue(_battle.RoundFight(_card1, _card2) == Outcome.winnerA);
        }
        [Test]
        public void RoundFigth_KnightvsWaterSpellShouldBeWaterSpell()
        {
            _card1 = new NormalKnight(10);
            _card2 = new WaterSpell(10);
            Assert.IsTrue(_battle.RoundFight(_card1, _card2) == Outcome.winnerB);
        }
        [Test]
        public void RoundFigth_KrakenvsSpellShouldBeKraken()
        {
            _card1 = new WaterKraken(10);
            _card2 = new NormalSpell(10);
            Assert.IsTrue(_battle.RoundFight(_card1, _card2) == Outcome.winnerA);
        }
        [Test]
        public void RoundFigth_ElvevsDragonShouldBeElve()
        {
            _card1 = new FireElves(10);
            _card2 = new FireDragon(10);
            Assert.IsTrue(_battle.RoundFight(_card1, _card2) == Outcome.winnerA);
        }


        //--------------Testcases for Change of Cards after round---------------
        [Test]
        public void PlayOneRound_ShouldChangeCardCount()
        {

            _mockBattle.Object.PlayOneRound(_deckA, _deckB);

            Assert.IsTrue(_deckA.getSizeStack()== 5 && _deckB.getSizeStack() == 3);
        }
        
        [Test]
        public void PlayOneRound_FourTimes_ShouldChangeCardCount()
        {
            _mockBattle.Object.PlayOneRound(_deckA, _deckB);
            _mockBattle.Object.PlayOneRound(_deckA, _deckB);
            _mockBattle.Object.PlayOneRound(_deckA, _deckB);
            _mockBattle.Object.PlayOneRound(_deckA, _deckB);

            Assert.IsTrue(_deckA.getSizeStack() == 8 && _deckB.getSizeStack() == 0);
        }
        
        [Test]
        public void startFight_shouldBePlayerA()
        {
            var result = _mockBattle.Object.startFight();
            
            Assert.IsTrue(result == Outcome.winnerA);
        }

        [Test]
        public void startFight_shouldBeDeuce()
        {
            List<AbstractCard> listA = new List<AbstractCard>() { new WaterGoblin(10), new WaterGoblin(10), new WaterGoblin(10), new WaterGoblin(10) };
            List<AbstractCard> listB = new List<AbstractCard>() { new WaterGoblin(10), new WaterGoblin(10), new WaterGoblin(10), new WaterGoblin(10) };
            AbstractDeckManager deckA = new Deck(listA);
            AbstractDeckManager deckB = new Deck(listB);
            Players playerA = new Players("PlayerA");
            Players playerB = new Players("PlayerB");
            playerA.MyDeck = deckA;
            playerB.MyDeck = deckB;
            _battle = new Fight(playerA, playerB);

            Assert.IsTrue(_battle.startFight() == Outcome.deuce);
        }
    }
}
