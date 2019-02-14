using NUnit.Framework;
using WizardsCastle.Logic.Data;
using WizardsCastle.Logic.Services;
using WizardsCastle.Logic.Tests.Helpers;

namespace WizardsCastle.Logic.Tests.Services
{
    [TestFixture]
    public class CombatServiceTests
    {
        private MockGameTools _tools;
        private CombatService _service;
        private Player _player;

        [SetUp]
        public void Setup()
        {
            _tools = new MockGameTools();
            _service = new CombatService(_tools);

            _player = Any.Player();
            _player.Dexterity = 10;
        }

        [TestCase(2)]
        [TestCase(9)]
        [TestCase(10)]
        public void PlayerGoesFirstIfInitiativeRollSucceeds(int initiativeRoll)
        {
            _tools.RandomizerMock.Setup(r => r.RollDice(2, 9)).Returns(initiativeRoll);

            Assert.That(_service.PlayerGoesFirst(_player), Is.True);
        }

        [TestCase(11)]
        [TestCase(18)]
        public void PlayerGoesSecondIfInitiativeRollFails(int initiativeRoll)
        {
            _tools.RandomizerMock.Setup(r => r.RollDice(2, 9)).Returns(initiativeRoll);

            Assert.That(_service.PlayerGoesFirst(_player), Is.False);
        }

        [Test]
        public void PlayerDoesNotGoFirstIfBlind()
        {
            _player.IsBlind = true;
            _tools.RandomizerMock.Setup(r => r.RollDice(2, 9)).Returns(1);

            Assert.That(_service.PlayerGoesFirst(_player), Is.False);
        }

        [Test]
        public void PlayerDoesNotGoFirstIfCursedWithLethargy()
        {
            _tools.CurseEvaluatorMock.Setup(c => c.IsEffectedByCurse(_player, Curses.CurseOfLethargy)).Returns(true);
            _tools.RandomizerMock.Setup(r => r.RollDice(2, 9)).Returns(1);

            Assert.That(_service.PlayerGoesFirst(_player), Is.False);
        }
    }
}