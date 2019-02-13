using System;
using Moq;
using NUnit.Framework;
using WizardsCastle.Logic.Data;
using WizardsCastle.Logic.Situations;
using WizardsCastle.Logic.Tests.Helpers;

namespace WizardsCastle.Logic.Tests.Situations
{
    [TestFixture]
    public class EnterRoomSituationTests
    {
        private ISituation _situation;
        private MockGameTools _tools;
        private GameData _data;
        private Location _location;

        [SetUp]
        public void Setup()
        {
            _location = Any.Location();
            _situation = new EnterRoomSituation(_location);

            _tools = new MockGameTools();
            _data = Any.GameData();
        }

        [Test]
        public void SetsLocationAndIncrementsTurnCounter()
        {
            var expectedCounter = _data.TurnCounter + 1;
            _data.Map.SetLocationInfo(_location, MapCodes.Entrance);

            _situation.PlayThrough(_data, _tools);

            Assert.That(_data.TurnCounter, Is.EqualTo(expectedCounter));
        }

        [Test]
        public void UnexploredRoomBecomesExplored()
        {
            _data.Map.SetLocationInfo(_location, MapCodes.Unexplored(MapCodes.EmptyRoom));

            _situation.PlayThrough(_data, _tools);

            var info = _data.Map.GetLocationInfo(_location);
            Assert.That(info, Is.EqualTo(MapCodes.EmptyRoom.ToString()));
        }

        [Test]
        public void InvalidMapCodeThrowsException()
        {
            _data.Map.SetLocationInfo(_location, "***");

            Assert.Throws<ArgumentException>(() => _situation.PlayThrough(_data, _tools));
        }

        [Test]
        public void EmptyRoomMessageThenNavigation()
        {
            TestDescribeAndLeaveRoom(MapCodes.EmptyRoom, Messages.EmptyRoomDescription);
        }

        [Test]
        public void EntranceMessageThenNavigation()
        {
            TestDescribeAndLeaveRoom(MapCodes.Entrance, Messages.EntranceDescription);
        }

        [Test]
        public void StairsDownMessageThenNavigation()
        {
            TestDescribeAndLeaveRoom(MapCodes.StairsDown, Messages.StairsDown);
        }

        [Test]
        public void StairsUpMessageThenNavigation()
        {
            TestDescribeAndLeaveRoom(MapCodes.StairsUp, Messages.StairsUp);
        }

        private void TestDescribeAndLeaveRoom(char roomCode, string expectedDescription)
        {
            _data.Map.SetLocationInfo(_location, roomCode);
            var next = Mock.Of<ISituation>();
            _tools.SituationBuilderMock.Setup(b => b.LeaveRoom()).Returns(next);

            var actual = _situation.PlayThrough(_data, _tools);

            Assert.That(actual, Is.SameAs(next));
            _tools.UIMock.Verify(ui => ui.DisplayMessage(expectedDescription));
        }
    }
}