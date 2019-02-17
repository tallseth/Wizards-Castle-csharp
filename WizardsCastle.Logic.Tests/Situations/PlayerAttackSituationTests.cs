using NUnit.Framework;
using WizardsCastle.Logic.Combat;
using WizardsCastle.Logic.Data;
using WizardsCastle.Logic.Services;
using WizardsCastle.Logic.Situations;
using WizardsCastle.Logic.Tests.Helpers;

namespace WizardsCastle.Logic.Tests.Situations
{
    [TestFixture]
    public class PlayerAttackSituationTests
    {
        private ISituation _situation;
        private MockGameTools _tools;
        private GameData _data;
        private Enemy _enemy;

        [SetUp]
        public void Setup()
        {
            _situation = new PlayerAttackSituation();

            _tools = new MockGameTools();
            _data = Any.GameData();

            _enemy = _tools.SetupEnemyAtCurrentLocation(_data);
        }

        [Test]
        public void PlayerMissesShowsMessageAndGoesToMonsterTurn()
        {
            var next = _tools.SetupNextSituation(sb => sb.EnemyAttack(false));
            _tools.CombatServiceMock.Setup(c => c.PlayerAttacks(_data.Player, _enemy)).Returns(new CombatResult { AttackerMissed = true});

            var actual = _situation.PlayThrough(_data, _tools);

            Assert.That(actual, Is.SameAs(next));
            _tools.UIMock.Verify(ui=>ui.DisplayMessage("You miss!"));
        }

        [Test]
        public void PlayerHitsShowsMessageAppliesDamageAndGoesToMonsterTurn()
        {
            var next = _tools.SetupNextSituation(sb => sb.EnemyAttack(false));
            var combatResult = new CombatResult { DamageToDefender = Any.Number()};
            _tools.CombatServiceMock.Setup(c => c.PlayerAttacks(_data.Player, _enemy)).Returns(combatResult);

            var actual = _situation.PlayThrough(_data, _tools);

            Assert.That(actual, Is.SameAs(next));
            _tools.UIMock.Verify(ui=>ui.DisplayMessage("You hit the " + _enemy.Name + " with your " + _data.Player.Weapon.Name + " for " + combatResult.DamageToDefender  + "!"));
        }

        [Test]
        public void PlayerKillsEnemyShowsMessageAndGoesToVictory()
        {
            _enemy.HitPoints = _data.Player.Weapon.Damage;
            var combatResult = new CombatResult { DamageToDefender = Any.Number(), DefenderDied = true};
            var next = _tools.SetupNextSituation(sb => sb.CombatVictory());
            _tools.CombatServiceMock.Setup(c => c.PlayerAttacks(_data.Player, _enemy)).Returns(combatResult);

            var actual = _situation.PlayThrough(_data, _tools);

            Assert.That(actual, Is.SameAs(next));
            _tools.UIMock.Verify(ui=>ui.DisplayMessage("You hit the " + _enemy.Name + " with your " + _data.Player.Weapon.Name + " for " + combatResult.DamageToDefender + "!"));
        }
    }
}