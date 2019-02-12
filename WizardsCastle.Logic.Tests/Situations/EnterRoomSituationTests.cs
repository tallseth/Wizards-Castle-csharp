using System;
using System.Linq;
using Moq;
using NUnit.Framework;
using WizardsCastle.Logic.Data;
using WizardsCastle.Logic.Situations;
using WizardsCastle.Logic.Tests.Helpers;
using WizardsCastle.Logic.UI;

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
            TestDescribeAndMoveRoom(MapCodes.EmptyRoom, NavigationOptions.Standard, Messages.EmptyRoomDescription);
        }

        [Test]
        public void EntranceMessageThenNavigation()
        {
            TestDescribeAndMoveRoom(MapCodes.Entrance, NavigationOptions.Entrance, Messages.EntranceDescription);
        }

        [Test]
        public void StairsDownMessageThenNavigation()
        {
            TestDescribeAndMoveRoom(MapCodes.StairsDown, NavigationOptions.Standard.Union(new []{NavigationOptions.StairsDown}).ToArray(), Messages.StairsDown);
        }

        [Test]
        public void StairsUpMessageThenNavigation()
        {
            TestDescribeAndMoveRoom(MapCodes.StairsUp, NavigationOptions.Standard.Union(new []{NavigationOptions.StairsUp}).ToArray(), Messages.StairsUp);
        }

        private void TestDescribeAndMoveRoom(char roomCode, UserOption[] expectedOptions, string expectedDescription)
        {
            _data.Map.SetLocationInfo(_location, roomCode);
            var next = Mock.Of<ISituation>();
            _tools.SituationBuilderMock.Setup(b => b.Navigate(expectedOptions)).Returns(next);

            var actual = _situation.PlayThrough(_data, _tools);

            Assert.That(actual, Is.SameAs(next));
            _tools.UIMock.Verify(ui => ui.DisplayMessage(expectedDescription));
        }
    }
}