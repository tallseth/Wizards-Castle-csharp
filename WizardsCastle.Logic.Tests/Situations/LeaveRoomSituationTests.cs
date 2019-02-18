using System.Collections.Generic;
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
    public class LeaveRoomSituationTests
    {
        private ISituation _situation;
        private MockGameTools _tools;
        private GameData _data;

        [SetUp]
        public void Setup()
        {
            _situation = new LeaveRoomSituation();

            _tools = new MockGameTools();
            _data = Any.GameData();
        }

        [Test]
        public void EmptyRoomMessageThenNavigation()
        {
            TestCorrectNavigationOptions(MapCodes.EmptyRoom, NavigationOptions.Standard);
        }

        [Test]
        public void EntranceMessageThenNavigation()
        {
            TestCorrectNavigationOptions(MapCodes.Entrance, NavigationOptions.Entrance);
        }

        [Test]
        public void StairsDownMessageThenNavigation()
        {
            TestCorrectNavigationOptions(MapCodes.StairsDown,  NavigationOptions.Standard.Add(NavigationOptions.StairsDown));
        }

        [Test]
        public void StairsUpMessageThenNavigation()
        {
            TestCorrectNavigationOptions(MapCodes.StairsUp, NavigationOptions.Standard.Add(NavigationOptions.StairsUp));
        }

        [Test]
        public void TeleportIsNavigationOptionIfPlayerHasRunestaff()
        {
            _data.Player.HasRuneStaff = true;
            Assert.That(GetActualOptionsUsed(MapCodes.EmptyRoom).Last(), Is.SameAs(NavigationOptions.Teleport));
            Assert.That(GetActualOptionsUsed(MapCodes.Entrance).Last(), Is.SameAs(NavigationOptions.Teleport));
            Assert.That(GetActualOptionsUsed(MapCodes.StairsUp).Last(), Is.SameAs(NavigationOptions.Teleport));
            Assert.That(GetActualOptionsUsed(MapCodes.StairsDown).Last(), Is.SameAs(NavigationOptions.Teleport));
        }

        private void TestCorrectNavigationOptions(char roomCode, UserOption[] expectedOptions)
        {
            _data.Player.HasRuneStaff = false;
            var actualOptions = GetActualOptionsUsed(roomCode);
            Assert.That(actualOptions, Is.EqualTo(expectedOptions));
        }

        private UserOption[] GetActualOptionsUsed(char roomCode)
        {
            _data.Map.SetLocationInfo(_data.CurrentLocation, roomCode);
            var next = Mock.Of<ISituation>();
            var actualOptions = new UserOption[0];
            _tools.SituationBuilderMock.Setup(b => b.Navigate(It.IsAny<UserOption[]>()))
                .Callback((IEnumerable<UserOption> used) => actualOptions = (UserOption[]) used)
                .Returns(next);

            var actual = _situation.PlayThrough(_data, _tools);

            Assert.That(actual, Is.SameAs(next));
            return actualOptions;
        }
    }
}