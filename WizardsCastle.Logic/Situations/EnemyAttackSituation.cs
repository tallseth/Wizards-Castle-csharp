using WizardsCastle.Logic.Data;

namespace WizardsCastle.Logic.Situations
{
    internal class EnemyAttackSituation : ISituation
    {
        private readonly bool _playerRetreating;

        public EnemyAttackSituation(bool playerRetreating)
        {
            _playerRetreating = playerRetreating;
        }

        public ISituation PlayThrough(GameData data, GameTools tools)
        {
            var enemy = tools.EnemyProvider.GetEnemy(data.Map, data.CurrentLocation);
            var result = tools.CombatService.EnemyAttacks(data.Player, enemy);

            var suffix = _playerRetreating ? " one last time as you retreat" : "";
            tools.UI.DisplayMessage($"The {enemy.Name} attacks{suffix}.");

            if (result.AttackerMissed)
            {
                tools.UI.DisplayMessage($"The {enemy.Name} misses!");

                return GetNext(tools);
            }

            tools.UI.DisplayMessage($"The {enemy.Name} hit you for {result.DamageToDefender} damage!");

            if (result.DefenderDied)
                return tools.SituationBuilder.GameOver($"You have been defeated by the {enemy.Name}.");

            return GetNext(tools);
        }

        private ISituation GetNext(GameTools tools)
        {
            if (_playerRetreating)
                return tools.SituationBuilder.LeaveRoom();

            return tools.SituationBuilder.CombatOptions();
        }
    }
}