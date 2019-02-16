using WizardsCastle.Logic.Data;

namespace WizardsCastle.Logic.Situations
{
    internal class EnemyAttackSituation : ISituation
    {
        public ISituation PlayThrough(GameData data, GameTools tools)
        {
            var enemy = tools.EnemyProvider.GetEnemy(data.Map, data.CurrentLocation);
            var result = tools.CombatService.EnemyAttacks(data.Player, enemy);

            if (result.AttackerMissed)
            {
                tools.UI.DisplayMessage($"The {enemy.Name} misses!");

                return tools.SituationBuilder.CombatOptions();
            }

            tools.UI.DisplayMessage($"The {enemy.Name} hit you for {result.DamageToDefender} damage!");

            return result.DefenderDied 
                ? tools.SituationBuilder.GameOver($"You have been defeated by the {enemy.Name}.") 
                : tools.SituationBuilder.CombatOptions();
        }
    }
}