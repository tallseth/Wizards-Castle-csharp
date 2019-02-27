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
            TestCorrectNavigationOptions(MapCodes.EmptyRoom, NavigationOptions.Standard.Add(NavigationOptions.Map));
        }

        [Test]
        public void EntranceMessageThenNavigation()
        {
            TestCorrectNavigationOptions(MapCodes.Entrance, NavigationOptions.Entrance.Add(NavigationOptions.Map));
        }

        [Test]
        public void StairsDownMessageThenNavigation()
        {
            TestCorrectNavigationOptions(MapCodes.StairsDown,  NavigationOptions.Standard.Add(NavigationOptions.StairsDown, NavigationOptions.Map));
        }

        [Test]
        public void StairsUpMessageThenNavigation()
        {
            TestCorrectNavigationOptions(MapCodes.StairsUp, NavigationOptions.Standard.Add(NavigationOptions.StairsUp, NavigationOptions.Map));
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

        [Test]
        public void ShineLampIsOptionIfPlayerHasLamp()
        {
            _data.Player.HasLamp = true;
            Assert.That(GetActualOptionsUsed(MapCodes.EmptyRoom), Contains.Item(NavigationOptions.ShineLamp));
            Assert.That(GetActualOptionsUsed(MapCodes.Entrance), Contains.Item(NavigationOptions.ShineLamp));
            Assert.That(GetActualOptionsUsed(MapCodes.StairsUp), Contains.Item(NavigationOptions.ShineLamp));
            Assert.That(GetActualOptionsUsed(MapCodes.StairsDown), Contains.Item(NavigationOptions.ShineLamp));
        }

        
        [Test]
        public void CannotShineLampIfBlind()
        {
            _data.Player.HasLamp = true;
            _data.Player.IsBlind = true;
            Assert.That(GetActualOptionsUsed(MapCodes.EmptyRoom), Does.Not.Contain(NavigationOptions.ShineLamp));
            Assert.That(GetActualOptionsUsed(MapCodes.Entrance), Does.Not.Contain(NavigationOptions.ShineLamp));
            Assert.That(GetActualOptionsUsed(MapCodes.StairsUp), Does.Not.Contain(NavigationOptions.ShineLamp));
            Assert.That(GetActualOptionsUsed(MapCodes.StairsDown), Does.Not.Contain(NavigationOptions.ShineLamp));
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