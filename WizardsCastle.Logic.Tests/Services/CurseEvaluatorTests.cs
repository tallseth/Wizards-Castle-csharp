using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using WizardsCastle.Logic.Data;
using WizardsCastle.Logic.Services;

namespace WizardsCastle.Logic.Tests.Services
{
    [TestFixture]
    internal class CurseEvaluatorTests
    {
        private CurseEvaluator _curseEvaluator;

        [SetUp]
        public void Setup()
        {
            _curseEvaluator = new CurseEvaluator();
        }

        [TestCaseSource(nameof(AllCurses))]
        public void PlayerIsNotEffectedByCurseTheyDoNotHave(Curses cursePlayerDoesNotHave)
        {
            var player = Any.Player();
            player.Curses = AllCurses.Where(c => c != cursePlayerDoesNotHave).Aggregate(Curses.None, (x, y) => x | y);

            Assert.That(_curseEvaluator.IsEffectedByCurse(player, cursePlayerDoesNotHave), Is.False);
        }

        [TestCaseSource(nameof(AllCurses))]
        public void PlayerIsEffectedByCurseTheyDoHave(Curses curse)
        {
            var player = Any.Player();
            player.Curses = curse;

            Assert.That(_curseEvaluator.IsEffectedByCurse(player, curse), Is.True);
        }

        [TestCaseSource(nameof(AllCurses))]
        public void PlayerIsNotEffectedByCursesWhenTheyHaveOrbOfZot(Curses curse)
        {
            var player = Any.Player();
            player.HasOrbOfZot = true;
            player.Curses = AllCurses.Aggregate(Curses.None, (x, y) => x | y);

            Assert.That(_curseEvaluator.IsEffectedByCurse(player, curse), Is.False);
        }

        [TestCaseSource(nameof(AllCurses))]
        public void PlayerIsNotEffectedByCursesWhenTheyHaveRuneStaff(Curses curse)
        {
            var player = Any.Player();
            player.HasRuneStaff = true;
            player.Curses = AllCurses.Aggregate(Curses.None, (x, y) => x | y);

            Assert.That(_curseEvaluator.IsEffectedByCurse(player, curse), Is.False);
        }

        internal static IEnumerable<Curses> AllCurses => new[] {Curses.CurseOfForgetfulness, Curses.CurseOfLethargy, Curses.CurseOfTheLeech};
    }
}