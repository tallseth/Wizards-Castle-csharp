using NUnit.Framework;
using WizardsCastle.Logic.Combat;
using WizardsCastle.Logic.Data;
using WizardsCastle.Logic.Services;
using WizardsCastle.Logic.Tests.Helpers;

namespace WizardsCastle.Logic.Tests.Services
{
    [TestFixture]
    internal class CombatServiceTests
    {
        private MockGameTools _tools;
        private CombatService _service;
        private Player _player;
        private Enemy _enemy;

        [SetUp]
        public void Setup()
        {
            _tools = new MockGameTools();
            _service = new CombatService(_tools);

            _player = Any.Player();

            _enemy = Enemy.CreateMonster(Monster.Minotaur);
        }
        
        [Test]
        public void PlayerGoesFirstIfRollSucceeds()
        {
            _player.IsBlind = true;
            _tools.CombatDiceMock.Setup(d => d.RollToDodge(_player)).Returns(true);

            Assert.That(_service.PlayerGoesFirst(_player), Is.False);
        }

        [Test]
        public void PlayerDoesNotGoFirstIfRollFails()
        {
            _player.IsBlind = true;
            _tools.CombatDiceMock.Setup(d => d.RollToDodge(_player)).Returns(false);

            Assert.That(_service.PlayerGoesFirst(_player), Is.False);
        }

        [Test]
        public void PlayerDoesNotGoFirstIfCursedWithLethargy()
        {
            _tools.CurseEvaluatorMock.Setup(c => c.IsEffectedByCurse(_player, Curses.CurseOfLethargy)).Returns(true);
            _tools.RandomizerMock.Setup(r => r.RollDice(2, 9)).Returns(1);

            Assert.That(_service.PlayerGoesFirst(_player), Is.False);
        }

        [Test]
        public void PlayerAttackMissesIfRollFails()
        {
            _tools.CombatDiceMock.Setup(d => d.RollToHit(_player)).Returns(false);

            var result = _service.PlayerAttacks(_player, _enemy);

            Assert.That(result.AttackerMissed, Is.True);
        }

        [Test]
        public void PlayerAttackHitsIfRollSucceeds()
        {
            _tools.CombatDiceMock.Setup(d => d.RollToHit(_player)).Returns(true);

            var result = _service.PlayerAttacks(_player, _enemy);

            Assert.That(result.AttackerMissed, Is.False);
        }

        [Test]
        public void PlayerAttackDoesWeaponDamageIfHit()
        {
            _enemy.HitPoints = _player.Weapon.Damage + 1;
            _tools.CombatDiceMock.Setup(d => d.RollToHit(_player)).Returns(true);

            var result = _service.PlayerAttacks(_player, _enemy);

            Assert.That(result.AttackerMissed, Is.False);
            Assert.That(result.DefenderDied, Is.False);
            Assert.That(result.WeaponBroke, Is.False);
            Assert.That(result.DamageToDefender, Is.EqualTo(_player.Weapon.Damage));
            Assert.That(_enemy.HitPoints, Is.EqualTo(1));
        }

        [Test]
        public void PlayerAttackKillsEnemyIfHitHardEnough()
        {
            _enemy.HitPoints = _player.Weapon.Damage;
            _tools.CombatDiceMock.Setup(d => d.RollToHit(_player)).Returns(true);

            var result = _service.PlayerAttacks(_player, _enemy);

            Assert.That(result.AttackerMissed, Is.False);
            Assert.That(result.DefenderDied, Is.True);
            Assert.That(result.WeaponBroke, Is.False);
            Assert.That(result.DamageToDefender, Is.EqualTo(_player.Weapon.Damage));
            Assert.That(_enemy.HitPoints, Is.EqualTo(0));
        }


        [Test]
        public void EnemyAttackHitsIfRollToDodgeFails()
        {
            _tools.CombatDiceMock.Setup(d => d.RollToDodge(_player)).Returns(false);

            var result = _service.EnemyAttacks(_player, _enemy);

            Assert.That(result.AttackerMissed, Is.False);
        }

        [Test]
        public void EnemyAttackMissesIfRollToDodgeSucceeds()
        {

            _tools.CombatDiceMock.Setup(d => d.RollToDodge(_player)).Returns(true);

            var result = _service.EnemyAttacks(_player, _enemy);

            Assert.That(result.AttackerMissed, Is.True);
        }

        [Test]
        public void EnemyAttackAbsorbedByArmor()
        {
            var originalStrength = _player.Strength;
            var originalDurability = _enemy.Damage + 1;
            _player.Armor = new Armor(Any.String(),_enemy.Damage * 2, originalDurability);
            _tools.CombatDiceMock.Setup(d => d.RollToDodge(_player)).Returns(false);

            var result = _service.EnemyAttacks(_player, _enemy);

            Assert.That(result.AttackerMissed, Is.False);
            Assert.That(result.DefenderDied, Is.False);
            Assert.That(result.DamageToDefender, Is.EqualTo(0));
            Assert.That(_player.Strength, Is.EqualTo(originalStrength));
            Assert.That(_player.Armor.Durability, Is.EqualTo(originalDurability - _enemy.Damage));
        }

        [Test]
        public void EnemyAttackDamagesPlayer()
        {
            _player.Strength = _enemy.Damage + 1;
            var originalStrength = _player.Strength;
            var originalDurability = _enemy.Damage + 1;
            
            _player.Armor = new Armor(Any.String(), _enemy.Damage - 3, originalDurability);
            _tools.CombatDiceMock.Setup(d => d.RollToDodge(_player)).Returns(false);

            var result = _service.EnemyAttacks(_player, _enemy);

            Assert.That(result.AttackerMissed, Is.False);
            Assert.That(result.DefenderDied, Is.False);
            var expectedDamageDealt = _enemy.Damage - _player.Armor.Protection;
            Assert.That(result.DamageToDefender, Is.EqualTo(expectedDamageDealt));
            Assert.That(_player.Strength, Is.EqualTo(originalStrength - expectedDamageDealt));
            Assert.That(_player.Armor.Durability, Is.EqualTo(originalDurability - _player.Armor.Protection));
        }

        [Test]
        public void EnemyAttackKillsPlayerIfHitHardEnough()
        {
            _player.Strength = _enemy.Damage - 1;
            
            _player.Armor = new Armor(Any.String(), 1, Any.Number());
            _tools.CombatDiceMock.Setup(d => d.RollToDodge(_player)).Returns(false);

            var result = _service.EnemyAttacks(_player, _enemy);

            Assert.That(result.DefenderDied);
            Assert.That(_player.Strength, Is.EqualTo(0));
        }

        [Test]
        public void EnemyDestroysArmorIfDurabilityGoesToZero()
        {
            var originalDurability = _enemy.Damage;
            
            _player.Armor = new Armor(Any.String(), _enemy.Damage, originalDurability);
            _tools.CombatDiceMock.Setup(d => d.RollToDodge(_player)).Returns(false);

            var result = _service.EnemyAttacks(_player, _enemy);

            Assert.That(result.ArmorDestroyed);
            Assert.That(_player.Armor, Is.Null);
        }

        [Test]
        public void EnemyAttacksPlayerWithoutArmor()
        {
            _player.Strength = _enemy.Damage + 1;
            var originalStrength = _player.Strength;
            _player.Armor = null;
            _tools.CombatDiceMock.Setup(d => d.RollToDodge(_player)).Returns(false);

            var result = _service.EnemyAttacks(_player, _enemy);

            Assert.That(result.AttackerMissed, Is.False);
            Assert.That(result.DefenderDied, Is.False);
            Assert.That(result.ArmorDestroyed, Is.False);
            Assert.That(result.DamageToDefender, Is.EqualTo(_enemy.Damage));
            Assert.That(_player.Strength, Is.EqualTo(originalStrength - _enemy.Damage));
            Assert.That(_player.Armor, Is.EqualTo(null));
        }

        [Test]
        public void PlayerAttackBreaksPlayerWeaponAgainstSpecificEnemiesIfUnlucky()
        {
            _tools.CombatDiceMock.Setup(c => c.RollToHit(_player)).Returns(true);
            _tools.CombatDiceMock.Setup(c => c.RollForWeaponBreakage(_enemy)).Returns(true);
            var expectedDamage = _player.Weapon.Damage;

            var result = _service.PlayerAttacks(_player, _enemy);

            Assert.That(result.AttackerMissed, Is.False);
            Assert.That(result.WeaponBroke, Is.True);
            Assert.That(result.DamageToDefender, Is.EqualTo(expectedDamage));
            Assert.That(_player.Weapon, Is.Null);
        }

        [Test]
        public void ChestExplodingAbsorbedByArmor()
        {
            var damage = Any.Number();
            _tools.RandomizerMock.Setup(r => r.RollDie(6)).Returns(damage);
            var originalStrength = _player.Strength;
            var originalDurability = damage + 1;
            _player.Armor = new Armor(Any.String(), damage * 2, originalDurability);

            var result = _service.ChestExplodes(_player);

            Assert.That(result.AttackerMissed, Is.False);
            Assert.That(result.DefenderDied, Is.False);
            Assert.That(result.DamageToDefender, Is.EqualTo(0));
            Assert.That(_player.Strength, Is.EqualTo(originalStrength));
            Assert.That(_player.Armor.Durability, Is.EqualTo(originalDurability - damage));
        }

        [Test]
        public void ChestExplodingDamagesPlayer()
        {
            var damage = Any.Number();
            _tools.RandomizerMock.Setup(r => r.RollDie(6)).Returns(damage);
            _player.Strength = damage + 1;
            var originalStrength = _player.Strength;
            var originalDurability = damage + 1;
            
            _player.Armor = new Armor(Any.String(), damage - 3, originalDurability);

            var result = _service.ChestExplodes(_player);

            Assert.That(result.AttackerMissed, Is.False);
            Assert.That(result.DefenderDied, Is.False);
            var expectedDamageDealt = damage - _player.Armor.Protection;
            Assert.That(result.DamageToDefender, Is.EqualTo(expectedDamageDealt));
            Assert.That(_player.Strength, Is.EqualTo(originalStrength - expectedDamageDealt));
            Assert.That(_player.Armor.Durability, Is.EqualTo(originalDurability - _player.Armor.Protection));
        }

        [Test]
        public void ChestExplodingKillsPlayerIfHitHardEnough()
        {
            var damage = Any.Number();
            _tools.RandomizerMock.Setup(r => r.RollDie(6)).Returns(damage);
            _player.Strength = damage - 1;
            
            _player.Armor = new Armor(Any.String(), 1, Any.Number());

            var result = _service.ChestExplodes(_player);

            Assert.That(result.DefenderDied);
            Assert.That(_player.Strength, Is.EqualTo(0));
        }

        [Test]
        public void ChestExplodingDestroysArmorIfDurabilityGoesToZero()
        {
            var damage = Any.Number();
            _tools.RandomizerMock.Setup(r => r.RollDie(6)).Returns(damage);
            var originalDurability = damage;
            
            _player.Armor = new Armor(Any.String(), damage, originalDurability);
            _tools.CombatDiceMock.Setup(d => d.RollToDodge(_player)).Returns(false);

            var result = _service.ChestExplodes(_player);

            Assert.That(result.ArmorDestroyed);
            Assert.That(_player.Armor, Is.Null);
        }

        [Test]
        public void ChestExplodesOnPlayerWithoutArmor()
        {
            var damage = Any.Number();
            _tools.RandomizerMock.Setup(r => r.RollDie(6)).Returns(damage);
            _player.Strength = damage + 1;
            var originalStrength = _player.Strength;
            _player.Armor = null;
            _tools.CombatDiceMock.Setup(d => d.RollToDodge(_player)).Returns(false);

            var result = _service.ChestExplodes(_player);

            Assert.That(result.AttackerMissed, Is.False);
            Assert.That(result.DefenderDied, Is.False);
            Assert.That(result.ArmorDestroyed, Is.False);
            Assert.That(result.DamageToDefender, Is.EqualTo(damage));
            Assert.That(_player.Strength, Is.EqualTo(originalStrength - damage));
            Assert.That(_player.Armor, Is.EqualTo(null));
        }
    }
}