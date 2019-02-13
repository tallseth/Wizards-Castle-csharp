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

        private void TestCorrectNavigationOptions(char roomCode, UserOption[] options)
        {
            _data.Map.SetLocationInfo(_data.CurrentLocation, roomCode);
            var next = Mock.Of<ISituation>();
            _tools.SituationBuilderMock.Setup(b => b.Navigate(options)).Returns(next);

            var actual = _situation.PlayThrough(_data, _tools);

            Assert.That(actual, Is.SameAs(next));
        }
    }
}