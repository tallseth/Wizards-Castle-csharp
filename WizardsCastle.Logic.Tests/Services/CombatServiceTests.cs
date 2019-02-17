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
            _player.Dexterity = 10;

            _enemy = Enemy.CreateMonster(Monster.Minotaur);
            _enemy.StoneSkin = false;
        }

        [TestCase(2)]
        [TestCase(9)]
        [TestCase(10)]
        public void PlayerGoesFirstIfInitiativeRollSucceeds(int initiativeRoll)
        {
            _tools.RandomizerMock.Setup(r => r.RollDice(2, 9)).Returns(initiativeRoll);

            Assert.That(_service.PlayerGoesFirst(_player), Is.True);
        }

        [TestCase(11)]
        [TestCase(18)]
        public void PlayerGoesSecondIfInitiativeRollFails(int initiativeRoll)
        {
            _tools.RandomizerMock.Setup(r => r.RollDice(2, 9)).Returns(initiativeRoll);

            Assert.That(_service.PlayerGoesFirst(_player), Is.False);
        }

        [Test]
        public void PlayerDoesNotGoFirstIfBlind()
        {
            _player.IsBlind = true;
            _tools.RandomizerMock.Setup(r => r.RollDice(2, 9)).Returns(1);

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
        public void PlayerAttackMissesIfDexterityLessThanRoll()
        {
            _tools.RandomizerMock.Setup(r => r.RollDie(20)).Returns(_player.Dexterity + 1);

            var result = _service.PlayerAttacks(_player, _enemy);

            Assert.That(result.AttackerMissed, Is.True);
        }

        [TestCase(0)]
        [TestCase(1)]
        public void PlayerAttackHitsIfDexterityEqualsOrLessThanRoll(int rollLessThanDexByAmount)
        {
            _tools.RandomizerMock.Setup(r => r.RollDie(20)).Returns(_player.Dexterity - rollLessThanDexByAmount);

            var result = _service.PlayerAttacks(_player, _enemy);

            Assert.That(result.AttackerMissed, Is.False);
        }

        [Test]
        public void PlayerAttackDoesWeaponDamageIfHit()
        {
            _enemy.HitPoints = _player.Weapon.Damage + 1;
            _tools.RandomizerMock.Setup(r => r.RollDie(20)).Returns(_player.Dexterity - 1);

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
            _tools.RandomizerMock.Setup(r => r.RollDie(20)).Returns(_player.Dexterity - 1);

            var result = _service.PlayerAttacks(_player, _enemy);

            Assert.That(result.AttackerMissed, Is.False);
            Assert.That(result.DefenderDied, Is.True);
            Assert.That(result.WeaponBroke, Is.False);
            Assert.That(result.DamageToDefender, Is.EqualTo(_player.Weapon.Damage));
            Assert.That(_enemy.HitPoints, Is.EqualTo(0));
        }


        [Test]
        public void EnemyAttackHitsIfDexterityLessThanRoll()
        {
            _tools.RandomizerMock.Setup(r => r.RollDice(3,7)).Returns(_player.Dexterity + 1);

            var result = _service.EnemyAttacks(_player, _enemy);

            Assert.That(result.AttackerMissed, Is.False);
        }

        [TestCase(0)]
        [TestCase(1)]
        public void EnemyAttackMissesIfDexterityEqualsOrGreaterThanRoll(int rollLessThanDexByAmount)
        {
            _tools.RandomizerMock.Setup(r => r.RollDice(3,7)).Returns(_player.Dexterity - rollLessThanDexByAmount);

            var result = _service.EnemyAttacks(_player, _enemy);

            Assert.That(result.AttackerMissed, Is.True);
        }

        [Test]
        public void EnemyAttackAbsorbedByArmor()
        {
            var originalStrength = _player.Strength;
            var originalDurability = _enemy.Damage + 1;
            _player.Armor = new Armor(Any.String(),_enemy.Damage * 2, originalDurability);
            _tools.RandomizerMock.Setup(r => r.RollDice(3,7)).Returns(_player.Dexterity + 1);

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
            _tools.RandomizerMock.Setup(r => r.RollDice(3,7)).Returns(_player.Dexterity + 1);

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
            _tools.RandomizerMock.Setup(r => r.RollDice(3,7)).Returns(_player.Dexterity + 1);

            var result = _service.EnemyAttacks(_player, _enemy);

            Assert.That(result.DefenderDied);
            Assert.That(_player.Strength, Is.EqualTo(0));
        }

        [Test]
        public void EnemyDestroysArmorIfDurabilityGoesToZero()
        {
            var originalDurability = _enemy.Damage;
            
            _player.Armor = new Armor(Any.String(), _enemy.Damage, originalDurability);
            _tools.RandomizerMock.Setup(r => r.RollDice(3,7)).Returns(_player.Dexterity + 1);

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
            _tools.RandomizerMock.Setup(r => r.RollDice(3,7)).Returns(_player.Dexterity + 1);

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
            _enemy.StoneSkin = true;
            _tools.RandomizerMock.Setup(r => r.RollDie(20)).Returns(_player.Dexterity - 1);
            _tools.RandomizerMock.Setup(r => r.OneChanceIn(8)).Returns(true);
            var expectedDamage = _player.Weapon.Damage;

            var result = _service.PlayerAttacks(_player, _enemy);

            Assert.That(result.AttackerMissed, Is.False);
            Assert.That(result.WeaponBroke, Is.True);
            Assert.That(result.DamageToDefender, Is.EqualTo(expectedDamage));
            Assert.That(_player.Weapon, Is.Null);
        }
    }
}