using System;
using NUnit.Framework;
using WizardsCastle.Logic.Combat;
using WizardsCastle.Logic.Data;
using WizardsCastle.Logic.Services;
using WizardsCastle.Logic.Situations;
using WizardsCastle.Logic.Tests.Helpers;

namespace WizardsCastle.Logic.Tests.Situations
{
    [TestFixture]
    public class EnemyAttackSituationTests
    {
        private ISituation _situation;
        private MockGameTools _tools;
        private GameData _data;
        private Enemy _enemy;

        [SetUp]
        public void Setup()
        {
            _situation = new EnemyAttackSituation();

            _tools = new MockGameTools();
            _data = Any.GameData();

            _enemy = _tools.SetupEnemyAtCurrentLocation(_data);
        }

        [Test]
        public void EnemyMissesReportsMessageAndGoesToPlayersTurn()
        {
            _tools.CombatServiceMock.Setup(c => c.EnemyAttacks(_data.Player, _enemy)).Returns(new CombatResult { AttackerMissed = true});
            var next = _tools.SetupNextSituation(sb => sb.CombatOptions());

            var actual =_situation.PlayThrough(_data, _tools);

            Assert.That(actual, Is.SameAs(next));
            _tools.UIMock.Verify(ui=>ui.DisplayMessage("The " + _enemy.Name + " misses!"));
        }

        [Test]
        public void EnemyHitsReportsDamageAndGoesToPlayersTurn()
        {
            var result = new CombatResult { DefenderDied = false, DamageToDefender = Any.Number()};
            _tools.CombatServiceMock.Setup(c => c.EnemyAttacks(_data.Player, _enemy)).Returns(result);
            var next = _tools.SetupNextSituation(sb => sb.CombatOptions());

            var actual =_situation.PlayThrough(_data, _tools);

            Assert.That(actual, Is.SameAs(next));
            _tools.UIMock.Verify(ui=>ui.DisplayMessage("The " + _enemy.Name + " hit you for " + result.DamageToDefender  +  " damage!"));
        }

        [Test]
        public void EnemyHitsAndKillsPlayerGoesToGameOver()
        {
            var result = new CombatResult { DefenderDied = true, DamageToDefender = Any.Number()};
            _tools.CombatServiceMock.Setup(c => c.EnemyAttacks(_data.Player, _enemy)).Returns(result);
            var next = _tools.SetupNextSituation(sb => sb.GameOver("You have been defeated by the " + _enemy.Name + "."));

            var actual =_situation.PlayThrough(_data, _tools);

            Assert.That(actual, Is.SameAs(next));
            _tools.UIMock.Verify(ui=>ui.DisplayMessage("The " + _enemy.Name + " hit you for " + result.DamageToDefender  +  " damage!"));
        }
    }
}