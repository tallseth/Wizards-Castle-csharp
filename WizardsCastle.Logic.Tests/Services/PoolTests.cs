using System;
using Moq;
using NUnit.Framework;
using WizardsCastle.Logic.Data;
using WizardsCastle.Logic.Services;

namespace WizardsCastle.Logic.Tests.Services
{
    [TestFixture]
    public class PoolTests
    {
        private Mock<IRandomizer> _randomizer;
        private Pool _pool;
        private Player _player;

        [SetUp]
        public void Setup()
        {
            _randomizer = new Mock<IRandomizer>();
            _pool = new Pool(_randomizer.Object);

            _player = Any.Player();
        }

        [Test]
        public void FeelStronger()
        {
            SetupEffectRoll(1);
            var impact = SetupImpactRoll();

            AssertPoolEffect(p => p.Strength, Messages.Stronger, impact);
        }

        [Test]
        public void FeelWeaker()
        {
            SetupEffectRoll(2);
            var impact = -1 * SetupImpactRoll();

            AssertPoolEffect(p => p.Strength, Messages.Weaker, impact);
        }

        [Test]
        public void FeelSmarter()
        {
            SetupEffectRoll(3);
            var impact = SetupImpactRoll();

            AssertPoolEffect(p => p.Intelligence, Messages.Smarter, impact);
        }

        [Test]
        public void FeelDumber()
        {
            SetupEffectRoll(4);
            var impact = -1 * SetupImpactRoll();

            AssertPoolEffect(p => p.Intelligence, Messages.Dumber, impact);
        }

        
        [Test]
        public void FeelNimbler()
        {
            SetupEffectRoll(5);
            var impact = SetupImpactRoll();

            AssertPoolEffect(p => p.Dexterity, Messages.Nimbler, impact);
        }

        [Test]
        public void FeelClumsier()
        {
            SetupEffectRoll(6);
            var impact = -1 * SetupImpactRoll();

            AssertPoolEffect(p => p.Dexterity, Messages.Clumsier, impact);
        }

        private void AssertPoolEffect(Func<Player, int> getter, string expectedMessage, int impact)
        {
            var original = getter(_player);


            var message = _pool.DrinkFrom(_player);

            Assert.That(message, Is.EqualTo(expectedMessage));
            Assert.That(getter(_player), Is.EqualTo(original + impact));
        }

        private int SetupImpactRoll()
        {
            var roll = Any.Number();
            _randomizer.Setup(r => r.RollDie(3)).Returns(roll);
            return roll;
        }

        private void SetupEffectRoll(int i)
        {
            _randomizer.Setup(r => r.RollDie(6)).Returns(i);
        }
    }
}