using WizardsCastle.Logic.Data;

namespace WizardsCastle.Logic.Situations
{
    internal class CombatVictorySituation : ISituation
    {
        public ISituation PlayThrough(GameData data, GameTools tools)
        {
            var enemy = tools.EnemyProvider.GetEnemy(data.Map, data.CurrentLocation);
            tools.UI.DisplayMessage($"You have defeated the {enemy.Name}.");

            var message = enemy.IsMonster
                ? tools.LootCollector.CollectMonsterLoot(data.Player)
                : tools.LootCollector.CollectVendorLoot(data.Player);

            tools.UI.DisplayMessage(message);

            data.Map.SetLocationInfo(data.CurrentLocation, MapCodes.EmptyRoom);

            return tools.SituationBuilder.LeaveRoom();
        }
    }
}