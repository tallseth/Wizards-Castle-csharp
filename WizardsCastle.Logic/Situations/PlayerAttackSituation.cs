﻿using WizardsCastle.Logic.Data;

namespace WizardsCastle.Logic.Situations
{
    internal class PlayerAttackSituation : ISituation
    {
        public ISituation PlayThrough(GameData data, GameTools tools)
        {
            tools.UI.DisplayMessage("You attack.");

            var enemy = tools.EnemyProvider.GetEnemy(data.Map, data.CurrentLocation);
            var result = tools.CombatService.PlayerAttacks(data.Player, enemy);

            if (result.AttackerMissed)
            {
                tools.UI.DisplayMessage("You miss!");
                return tools.SituationBuilder.EnemyAttack(false);
            }

            if(!result.WeaponBroke)
                tools.UI.DisplayMessage($"You hit the {enemy.Name} with your {data.Player.Weapon.Name} for {result.DamageToDefender}!");
            else
                tools.UI.DisplayMessage($"You hit the {enemy.Name} for {result.DamageToDefender}, but your weapon broke!");

            return result.DefenderDied
                ? tools.SituationBuilder.CombatVictory() 
                : tools.SituationBuilder.EnemyAttack(false);
        }
    }
}