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
            _situation = new EnemyAttackSituation(Any.Bool());

            _tools = new MockGameTools();
            _data = Any.GameData();

            _enemy = _tools.SetupEnemyAtCurrentLocation(_data);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void EnemyMissesReportsMessageAndGoesToNextSituation(bool retreating)
        {
            _situation = new EnemyAttackSituation(retreating);
            var next = retreating 
                ? _tools.SetupNextSituation(sb=>sb.LeaveRoom()) 
                : _tools.SetupNextSituation(sb => sb.CombatOptions());

            _tools.CombatServiceMock.Setup(c => c.EnemyAttacks(_data.Player, _enemy)).Returns(new CombatResult { AttackerMissed = true});

            var actual =_situation.PlayThrough(_data, _tools);

            Assert.That(actual, Is.SameAs(next));
            _tools.UIMock.Verify(ui=>ui.DisplayMessage("The " + _enemy.Name + " misses!"));
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

        
        [TestCase(true)]
        [TestCase(false)]
        public void EnemyHitsReportsDamageAndGoesToNextSituation(bool retreating)
        {
            _situation = new EnemyAttackSituation(retreating);
            var next = retreating 
                ? _tools.SetupNextSituation(sb=>sb.LeaveRoom()) 
                : _tools.SetupNextSituation(sb => sb.CombatOptions());

            var result = new CombatResult { DefenderDied = false, DamageToDefender = Any.Number()};
            _tools.CombatServiceMock.Setup(c => c.EnemyAttacks(_data.Player, _enemy)).Returns(result);
            
            var actual =_situation.PlayThrough(_data, _tools);

            Assert.That(actual, Is.SameAs(next));
            _tools.UIMock.Verify(ui=>ui.DisplayMessage("The " + _enemy.Name + " hit you for " + result.DamageToDefender  +  " damage!"));
        }
    }
}