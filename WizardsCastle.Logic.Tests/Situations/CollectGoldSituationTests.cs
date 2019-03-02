using NUnit.Framework;
using WizardsCastle.Logic.Data;
using WizardsCastle.Logic.Situations;
using WizardsCastle.Logic.Tests.Helpers;

namespace WizardsCastle.Logic.Tests.Situations
{
    [TestFixture]
    public class CollectGoldSituationTests
    {
        private ISituation _situation;
        private MockGameTools _tools;
        private GameData _data;
        private Location _location;

        [SetUp]
        public void Setup()
        {
            _location = Any.Location();
            _situation = new SituationBuilder().CollectGold();

            _tools = new MockGameTools();
            _data = Any.GameData();
        }

        [Test]
        public void PlayerLeavesRoomNext()
        {
            var next = _tools.SetupNextSituation(sb => sb.LeaveRoom());

            var actual = _situation.PlayThrough(_data, _tools);

            Assert.That(actual, Is.SameAs(next));
        }

        [Test]
        public void PlayerGetsGold()
        {
            var original = _data.Player.GoldPieces;
            var found = Any.Number();
            _tools.RandomizerMock.Setup(r => r.RollDie(10)).Returns(found);

            _situation.PlayThrough(_data, _tools);

            Assert.That(_data.Player.GoldPieces, Is.EqualTo(original + found));
        }

        [Test]
        public void RoomIsMarkedEmpty()
        {
            _data.Map.SetLocationInfo(_data.CurrentLocation, Any.String());

            _situation.PlayThrough(_data, _tools);

            Assert.That(_data.Map.GetLocationInfo(_data.CurrentLocation), Is.EqualTo(MapCodes.EmptyRoom.ToString()));
        }
    }

    [TestFixture]
    public class DrinkFromPoolSituationTests
    {
        private ISituation _situation;
        private MockGameTools _tools;
        private GameData _data;
        private Location _location;

        [SetUp]
        public void Setup()
        {
            _location = Any.Location();
            _situation = new SituationBuilder().DrinkFromPool();

            _tools = new MockGameTools();
            _data = Any.GameData();
        }

        [Test]
        public void PlayerLeavesRoomNext()
        {
            var next = _tools.SetupNextSituation(sb => sb.LeaveRoom());

            var actual = _situation.PlayThrough(_data, _tools);

            Assert.That(actual, Is.SameAs(next));
        }

        [Test]
        public void DisplaysMessageFromDrinking()
        {
            var message = Any.String();
            _tools.PoolMock.Setup(p => p.DrinkFrom(_data.Player)).Returns(message);

            _situation.PlayThrough(_data, _tools);

            _tools.UIMock.Verify(ui=>ui.DisplayMessage(message));
            _tools.UIMock.Verify(ui=>ui.PromptUserAcknowledgement());
        }

        [Test]
        public void GameOverIfPlayerDiesFromDrinking()
        {
            _tools.PoolMock.Setup(p => p.DrinkFrom(_data.Player))
                .Callback((Player p) => p.Strength = 0);

            var next = _tools.SetupNextSituation(sb => sb.GameOver(Messages.DieFromPool));

            var actual = _situation.PlayThrough(_data, _tools);

            Assert.That(actual, Is.SameAs(next));

        }
    }
}