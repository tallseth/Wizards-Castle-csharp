using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using WizardsCastle.Logic.Data;
using WizardsCastle.Logic.Situations;
using WizardsCastle.Logic.Tests.Helpers;
using WizardsCastle.Logic.UI;

namespace WizardsCastle.Logic.Tests.Situations
{
    [TestFixture]
    internal class NavigationSituationTests
    {
        private ISituation _situation;
        private MockGameTools _tools;
        private GameData _data;
        private UserOption[] _movementOptions;

        [SetUp]
        public void Setup()
        {
            _movementOptions = Any.UserOptions();
            _situation = new SituationBuilder().Navigate(_movementOptions);

            _tools = new MockGameTools();
            _data = Any.GameData();
        }

        [Test]
        public void NonExitMoveEntersDifferentRoom()
        {
            var move = Any.RegularMove();
            var location = Any.Location();

            SetupMove(move);
            _tools.MoveInterpreterMock.Setup(m => m.GetTargetLocation(_data.CurrentLocation, move)).Returns(location);
            var next = _tools.SetupNextSituation(b => b.EnterRoom(location));

            var actual = _situation.PlayThrough(_data, _tools);

            Assert.That(actual, Is.SameAs(next));
        }

        [Test]
        public void ExitWithOrbWins()
        {
            _data.Player.HasOrbOfZot = true;
            SetupMove(Move.Exit);
            var next = _tools.SetupNextSituation(b => b.GameOver(Messages.WinTheGame));

            var actual = _situation.PlayThrough(_data, _tools);

            Assert.That(actual, Is.SameAs(next));
        }

        [Test]
        public void ExitWithoutOrbLoses()
        {
            _data.Player.HasOrbOfZot = false;
            SetupMove(Move.Exit);
            var next = _tools.SetupNextSituation(b => b.GameOver(Messages.LostByRetreat));

            var actual = _situation.PlayThrough(_data, _tools);

            Assert.That(actual, Is.SameAs(next));
        }

        [Test]
        public void ShowMapMoveShowsMap()
        {
            SetupMove(Move.ShowMap);
            var next = _tools.SetupNextSituation(sb => sb.ShowMap());

            var actual = _situation.PlayThrough(_data, _tools);

            Assert.That(actual, Is.SameAs(next));
        }

        [Test]
        public void ShineLampDoesThat()
        {
            SetupMove(Move.ShineLamp);
            var next = _tools.SetupNextSituation(sb => sb.ShineLamp());

            var actual = _situation.PlayThrough(_data, _tools);

            Assert.That(actual, Is.SameAs(next));
        }

        [Test]
        public void TeleportMoveDoesThat()
        {
            SetupMove(Move.Teleport);
            var next = _tools.SetupNextSituation(sb => sb.Teleport());

            var actual = _situation.PlayThrough(_data, _tools);

            Assert.That(actual, Is.SameAs(next));            
        }

        [Test]
        public void UpdatesLastMoveOnGameData()
        {
            var move = Any.RegularMove();
            SetupMove(move);
            _data.LastMove = Move.Exit;

            _situation.PlayThrough(_data, _tools);

            Assert.That(_data.LastMove, Is.EqualTo(move));
        }

        private void SetupMove(Move move)
        {
            _tools.UIMock.Setup(ui => ui.PromptUserChoice(_movementOptions, true)).Returns(Any.UserOptionWithValue(move));
        }
    }
}