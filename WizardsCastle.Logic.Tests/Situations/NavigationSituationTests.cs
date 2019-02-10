using Moq;
using NUnit.Framework;
using WizardsCastle.Logic.Data;
using WizardsCastle.Logic.Situations;
using WizardsCastle.Logic.Tests.Helpers;
using WizardsCastle.Logic.UI;

namespace WizardsCastle.Logic.Tests.Situations
{
    [TestFixture]
    public class NavigationSituationTests
    {
        private ISituation _situation;
        private MockGameTools _tools;
        private GameData _data;
        private UserOption[] _movementOptions;

        [SetUp]
        public void Setup()
        {
            _movementOptions = Any.UserOptions();
            _situation = new NavigationSituation(_movementOptions);

            _tools = new MockGameTools();
            _data = Any.GameData();
        }

        [Test]
        public void NonExitMoveEntersDifferentRoom()
        {
            var move = Any.InsideMove();
            var next = Mock.Of<ISituation>();
            var location = Any.Location();

            SetupMove(move);
            _tools.MoveInterpreterMock.Setup(m => m.GetTargetLocation(_data.CurrentLocation, move)).Returns(location);
            _tools.SituationBuilderMock.Setup(b => b.EnterRoom(location)).Returns(next);

            var actual = _situation.PlayThrough(_data, _tools);

            Assert.That(actual, Is.SameAs(next));
        }

        [Test]
        public void ExitWithOrbWins()
        {
            _data.Player.HasOrbOfZot = true;
            var next = Mock.Of<ISituation>();
            SetupMove(Move.Exit);
            _tools.SituationBuilderMock.Setup(b => b.GameOver(Messages.WinTheGame)).Returns(next);

            var actual = _situation.PlayThrough(_data, _tools);

            Assert.That(actual, Is.SameAs(next));
        }

        [Test]
        public void ExitWithoutOrbLoses()
        {
            _data.Player.HasOrbOfZot = false;
            var next = Mock.Of<ISituation>();
            SetupMove(Move.Exit);
            _tools.SituationBuilderMock.Setup(b => b.GameOver(Messages.LostByRetreat)).Returns(next);

            var actual = _situation.PlayThrough(_data, _tools);

            Assert.That(actual, Is.SameAs(next));
        }

        private void SetupMove(Move move)
        {
            _tools.UIMock.Setup(ui => ui.PromptUserChoice(_movementOptions)).Returns(Any.UserOptionWithValue(move));
        }
    }
}