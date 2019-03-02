using NUnit.Framework;
using WizardsCastle.Logic.Data;
using WizardsCastle.Logic.Services;
using WizardsCastle.Logic.Situations;
using WizardsCastle.Logic.Tests.Helpers;

namespace WizardsCastle.Logic.Tests.Situations
{
    [TestFixture]
    public class OpenChestSituationTests
    {
        private ISituation _situation;
        private MockGameTools _tools;
        private GameData _data;
        private ISituation _next;

        [SetUp]
        public void Setup()
        {
            _situation = new SituationBuilder().OpenChest();

            _tools = new MockGameTools();
            _data = Any.GameData();

            _data.Map.SetLocationInfo(_data.CurrentLocation, Any.String());

            _next = _tools.SetupNextSituation(sb => sb.LeaveRoom());
        }

        [Test]
        public void PoisonGasChest()
        {
            _tools.RandomizerMock.Setup(r => r.RollDie(4)).Returns(1);

            var originalTurns = Any.Number();
            _data.TurnCounter = originalTurns;

            var actual = _situation.PlayThrough(_data, _tools);

            Assert.That(_data.TurnCounter, Is.EqualTo(originalTurns + 20));
            Assert.That(actual, Is.SameAs(_next));
            Assert.That(_data.Map.GetLocationInfo(_data.CurrentLocation), Is.EqualTo(MapCodes.EmptyRoom));
        }

        [Test]
        public void ExplodingChestDamagesPlayer()
        {
            _tools.RandomizerMock.Setup(r => r.RollDie(4)).Returns(2);
            _tools.CombatServiceMock.Setup(c => c.ChestExplodes(_data.Player)).Returns(new CombatResult {DefenderDied = false});

            var actual = _situation.PlayThrough(_data, _tools);

            Assert.That(actual, Is.SameAs(_next));
            Assert.That(_data.Map.GetLocationInfo(_data.CurrentLocation), Is.EqualTo(MapCodes.EmptyRoom));
        }

        [Test]
        public void ExplodingChestKillsPlayer()
        {
            _tools.RandomizerMock.Setup(r => r.RollDie(4)).Returns(2);
            _tools.CombatServiceMock.Setup(c => c.ChestExplodes(_data.Player)).Returns(new CombatResult {DefenderDied = true});
            var gameOver = _tools.SetupNextSituation(sb => sb.GameOver(Messages.KilledByExplodingChest));

            var actual = _situation.PlayThrough(_data, _tools);

            Assert.That(actual, Is.SameAs(gameOver));
        }

        [TestCase(3)]
        [TestCase(4)]
        public void TreasureChest(int diceRoll)
        {
            _tools.RandomizerMock.Setup(r => r.RollDie(4)).Returns(diceRoll);
            var originalGold = _data.Player.GoldPieces;
            var addedGold = Any.Number();
            _tools.RandomizerMock.Setup(r => r.RollDice(5, 200)).Returns(addedGold);

            var actual = _situation.PlayThrough(_data, _tools);

            Assert.That(_data.Player.GoldPieces, Is.EqualTo(originalGold + addedGold));
            Assert.That(actual, Is.SameAs(_next));
            Assert.That(_data.Map.GetLocationInfo(_data.CurrentLocation), Is.EqualTo(MapCodes.EmptyRoom));
        }
    }
}