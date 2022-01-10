using NUnit.Framework;
using MonsterCardGame;
using MonsterCardGame.Cards;

namespace MonsterCardGame.Test
{
    class TestAbstractCards
    {
        AbstractCard _card1, _card2;

        [SetUp]
        public void Setup()
        {
        }

        [TestCase(120, 100)]
        public void AdaptDamage_MonstervsMonster_ShouldBeUnchanged(int strength1, int strength2)
        {
            _card1 = new WaterGoblin(strength1);
            _card2 = new FireTroll(strength2);
            
            Assert.AreEqual(strength1, _card1.AdaptDamage(_card2), "input strengt should be adapted strength");
            Assert.AreEqual(strength2, _card2.AdaptDamage(_card1), "input strengt should be adapted strength");
        }

        [TestCase(10, 20)]
        public void AdaptDamage_SpellvsSpell_ShouldBeDoubleOrHalf(int strength1, int strength2)
        {
            _card1 = new FireSpell(strength1);
            _card2 = new WaterSpell(strength2);

            Assert.IsTrue(strength1/2 == _card1.AdaptDamage(_card2), "fire spell should be half");
            Assert.IsTrue(strength2*2 == _card2.AdaptDamage(_card1), "water should be doubled");
        }

        [TestCase(10, 20)]
        public void AdaptDamage_SpellvsMonster_ShouldBeDoubleOrHalf(int strength1, int strength2)
        {
            _card1 = new FireSpell(strength1);
            _card2 = new WaterGoblin(strength2);

            Assert.IsTrue(strength1 / 2 == _card1.AdaptDamage(_card2), "fire spell should be half");
            Assert.IsTrue(strength2 * 2 == _card2.AdaptDamage(_card1), "water should be doubled");
        }

        [TestCase(10, 20)]
        public void AdaptDamage_SpellvsMonster_ShouldBeUnchanged(int strength1, int strength2)
        {
            _card1 = new NormalKnight(strength1);
            _card2 = new NormalSpell(strength2);

            Assert.IsTrue(strength1 == _card1.AdaptDamage(_card2), "knight should be same");
            Assert.IsTrue(strength2 == _card2.AdaptDamage(_card1), "spell should be same");
        }
    }
}
