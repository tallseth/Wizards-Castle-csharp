using WizardsCastle.Logic.Data;

namespace WizardsCastle.Logic.Situations
{
    internal class PlayerAttackSituation : ISituation
    {
        public ISituation PlayThrough(GameData data, GameTools tools)
        {
            var enemy = tools.EnemyProvider.GetEnemy(data.Map, data.CurrentLocation);
            var result = tools.CombatService.PlayerAttacks(data.Player, enemy);

            if (result.AttackerMissed)
            {
                tools.UI.DisplayMessage("You miss!");
                return tools.SituationBuilder.EnemyAttack(false);
            }

            tools.UI.DisplayMessage($"You hit the {enemy.Name} with your {data.Player.Weapon.Name} for {result.DamageToDefender}!");

            return result.DefenderDied
                ? tools.SituationBuilder.CombatVictory() 
                : tools.SituationBuilder.EnemyAttack(false);
        }
    }
}