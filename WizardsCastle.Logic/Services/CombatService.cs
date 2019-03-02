using System;
using WizardsCastle.Logic.Combat;
using WizardsCastle.Logic.Data;

namespace WizardsCastle.Logic.Services
{
    internal interface ICombatService
    {
        bool PlayerGoesFirst(Player player);
        CombatResult PlayerAttacks(Player player, Enemy enemy);
        CombatResult EnemyAttacks(Player player, Enemy enemy);
        CombatResult ChestExplodes(Player player);
    }

    internal class CombatResult
    {
        internal bool AttackerMissed { get; set; }
        internal bool DefenderDied { get; set; }
        internal bool WeaponBroke { get; set; }
        internal bool ArmorDestroyed { get; set; }
        internal int DamageToDefender { get; set; }
    }

    internal class CombatService : ICombatService
    {
        private readonly GameTools _tools;

        public CombatService(GameTools tools)
        {
            _tools = tools;
        }

        public bool PlayerGoesFirst(Player player)
        {
            if (_tools.CurseEvaluator.IsEffectedByCurse(player, Curses.CurseOfLethargy))
                return false;

            return _tools.CombatDice.RollToGoFirst(player);
        }

        public CombatResult PlayerAttacks(Player player, Enemy enemy)
        {
            var dice = _tools.CombatDice;
            if(!dice.RollToHit(player))
                return new CombatResult { AttackerMissed = true};

            var damage = player.Weapon.Damage;
            enemy.HitPoints -= damage;

            var weaponBroke = dice.RollForWeaponBreakage(enemy);
            if (weaponBroke)
                player.Weapon = null;

            return new CombatResult
            {
                DamageToDefender = damage,
                DefenderDied = enemy.HitPoints < 1,
                WeaponBroke = weaponBroke
            };
        }

        public CombatResult EnemyAttacks(Player player, Enemy enemy)
        {
            if(_tools.CombatDice.RollToDodge(player))
                return new CombatResult { AttackerMissed = true};

            return ApplyDamage(player, enemy.Damage);
        }

        public CombatResult ChestExplodes(Player player)
        {
            return ApplyDamage(player, _tools.Randomizer.RollDie(6));
        }

        private static CombatResult ApplyDamage(Player player, int damage)
        {
            var armorDestroyed = false;

            var armor = player.Armor ?? new Armor("fake", 0, int.MaxValue);
            var damageToPlayer = Math.Max(0, damage - armor.Protection);
            var damageToArmor = Math.Min(damage, armor.Protection);

            player.Strength -= damageToPlayer;
            armor.Durability -= damageToArmor;

            if (armor.Durability < 1)
            {
                player.Armor = null;
                armorDestroyed = true;
            }

            return new CombatResult
            {
                DamageToDefender = damageToPlayer,
                ArmorDestroyed = armorDestroyed,
                DefenderDied = player.Strength < 1
            };
        }
    }
}