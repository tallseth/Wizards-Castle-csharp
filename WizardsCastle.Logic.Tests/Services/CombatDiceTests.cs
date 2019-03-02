using Moq;
using NUnit.Framework;
using WizardsCastle.Logic.Data;
using WizardsCastle.Logic.Services;

namespace WizardsCastle.Logic.Tests.Services
{
    [TestFixture]
    public class CombatDiceTests
    {
        private Mock<IRandomizer> _randomizer;
        private CombatDice _dice;
        private Player _player;

        [SetUp]
        public void Setup()
        {
            _randomizer = new Mock<IRandomizer>();
            _dice = new CombatDice(_randomizer.Object);

            _player = Any.Player();
        }

        [Test]
        public void RollToHitSuccess()
        {
            _randomizer.Setup(r => r.RollDie(20)).Returns(_player.Dexterity);

            Assert.That(_dice.RollToHit(_player), Is.True);
        }

        [Test]
        public void RollToHitFailure()
        {
            _randomizer.Setup(r => r.RollDie(20)).Returns(_player.Dexterity + 1);

            Assert.That(_dice.RollToHit(_player), Is.False);
        }

        [TestCase(0, false)]
        [TestCase(1, false)]
        [TestCase(2, false)]
        [TestCase(3, true)]
        public void RollToHitHarderIfBlind(int dexterityOffset, bool expectedHit)
        {
            _player.IsBlind = true;
            _randomizer.Setup(r => r.RollDie(20)).Returns(_player.Dexterity - dexterityOffset);

            Assert.That(_dice.RollToHit(_player), Is.EqualTo(expectedHit));
        }

        [Test]
        public void RollToDodgeSuccess()
        {
            _randomizer.Setup(r => r.RollDice(3, 7)).Returns(_player.Dexterity);

            Assert.That(_dice.RollToDodge(_player), Is.True);
        }

        [Test]
        public void RollToDodgeFailure()
        {
            _randomizer.Setup(r => r.RollDice(3, 7)).Returns(_player.Dexterity + 1);

            Assert.That(_dice.RollToDodge(_player), Is.False);
        }

        [TestCase(0, false)]
        [TestCase(1, false)]
        [TestCase(2, false)]
        [TestCase(3, true)]
        public void RollTDodgeHarderIfBlind(int dexterityOffset, bool expectedDodge)
        {
            _player.IsBlind = true;
            _randomizer.Setup(r => r.RollDice(3,7)).Returns(_player.Dexterity - dexterityOffset);

            Assert.That(_dice.RollToDodge(_player), Is.EqualTo(expectedDodge));
        }

        [Test]
        public void RollToGoFirstSuccess()
        {
            _randomizer.Setup(r => r.RollDice(2, 9)).Returns(_player.Dexterity);

            Assert.That(_dice.RollToGoFirst(_player), Is.True);
        }

        [Test]
        public void RollToGoFirstFailure()
        {
            _randomizer.Setup(r => r.RollDice(2, 9)).Returns(_player.Dexterity + 1);

            Assert.That(_dice.RollToGoFirst(_player), Is.False);
        }

        [Test]
        public void RollToGoFirstFailsIfBlind()
        {
            _player.IsBlind = true;
            _randomizer.Setup(r => r.RollDice(2, 9)).Returns(_player.Dexterity);

            Assert.That(_dice.RollToGoFirst(_player), Is.False);
        }

        [Test]
        public void RollForWeaponBreakageSucceedsForUnluckyRollAgainstStoneskinMonster()
        {
            var enemy = Any.Monster();
            enemy.StoneSkin = true;
            _randomizer.Setup(r => r.OneChanceIn(8)).Returns(true);

            Assert.That(_dice.RollForWeaponBreakage(enemy), Is.True);
        }

        [Test]
        public void RollForWeaponBreakageFailsForLuckyRollAgainstStoneskinMonster()
        {
            var enemy = Any.Monster();
            enemy.StoneSkin = true;
            _randomizer.Setup(r => r.OneChanceIn(8)).Returns(false);

            Assert.That(_dice.RollForWeaponBreakage(enemy), Is.False);
        }

        [Test]
        public void RollForWeaponBreakageFailsAgainstRegularMonster()
        {
            var enemy = Any.Monster();
            enemy.StoneSkin = false;
            _randomizer.Setup(r => r.OneChanceIn(8)).Returns(true);

            Assert.That(_dice.RollForWeaponBreakage(enemy), Is.False);
        }
    }
}