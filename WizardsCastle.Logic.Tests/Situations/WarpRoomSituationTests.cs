using NUnit.Framework;
using WizardsCastle.Logic.Data;
using WizardsCastle.Logic.Situations;
using WizardsCastle.Logic.Tests.Helpers;

namespace WizardsCastle.Logic.Tests.Situations
{
    [TestFixture]
    public class WarpRoomSituationTests
    {
        private MockGameTools _tools;
        private GameData _data;

        [SetUp]
        public void Setup()
        {
            _tools = new MockGameTools();
            _data = Any.GameData();
        }

        [Test]
        public void StandardWarpMovesToRandomRoom()
        {
            var situation = new WarpRoomSituation(false);

            var warpTarget = Any.Location();
            _tools.RandomizerMock.Setup(r => r.RandomLocation()).Returns(warpTarget);
            var next = _tools.SetupNextSituation(sb => sb.EnterRoom(warpTarget));

            Assert.That(situation.PlayThrough(_data, _tools), Is.SameAs(next));
        }

        [Test]
        public void WalkingIntoWarpOfZotWithoutRunestaffWalksOutTheOtherSide()
        {
            var situation = new WarpRoomSituation(true);
            _data.Player.HasRuneStaff = false;
            _data.LastMove = Any.RegularMove();

            var target = Any.Location();
            _tools.MoveInterpreterMock.Setup(r => r.GetTargetLocation(_data.CurrentLocation, _data.LastMove)).Returns(target);
            var next = _tools.SetupNextSituation(sb => sb.EnterRoom(target));
            
            Assert.That(situation.PlayThrough(_data, _tools), Is.SameAs(next));
        }

        [Test]
        public void TeleportingIntoWarpOfZotWithRunestaffGetsOrbOfZotAndClearsRoom()
        {
            var situation = new WarpRoomSituation(true);
            _data.Map.SetLocationInfo(_data.CurrentLocation, MapCodes.WarpOfZot);
            _data.Player.HasRuneStaff = true;
            _data.LastMove = Move.Teleport;

            var next = _tools.SetupNextSituation(sb => sb.LeaveRoom());
            
            Assert.That(situation.PlayThrough(_data, _tools), Is.SameAs(next));
            Assert.That(_data.Player.HasRuneStaff, Is.False);
            Assert.That(_data.Player.HasOrbOfZot, Is.True);
            Assert.That(_data.Map.GetLocationInfo(_data.CurrentLocation), Is.EqualTo(MapCodes.EmptyRoom.ToString()));
        }
    }
}