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
        private readonly ICombatDice _dice;

        public CombatService(GameTools tools)
        {
            _tools = tools;
            _dice = _tools.CombatDice;
        }

        public bool PlayerGoesFirst(Player player)
        {
            if (_tools.CurseEvaluator.IsEffectedByCurse(player, Curses.CurseOfLethargy))
                return false;

            return _dice.RollToGoFirst(player);
        }

        public CombatResult PlayerAttacks(Player player, Enemy enemy)
        {
            if(!_dice.RollToHit(player))
                return new CombatResult { AttackerMissed = true};

            var damage = player.Weapon.Damage;
            enemy.HitPoints -= damage;

            var weaponBroke = _dice.RollForWeaponBreakage(enemy);
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
            if(_dice.RollToDodge(player))
                return new CombatResult { AttackerMissed = true};

            var armorDestroyed = false;

            var armor = player.Armor ?? new Armor("fake", 0, int.MaxValue);
            var damageToPlayer = Math.Max(0, enemy.Damage - armor.Protection);
            var damageToArmor = Math.Min(enemy.Damage, armor.Protection);

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