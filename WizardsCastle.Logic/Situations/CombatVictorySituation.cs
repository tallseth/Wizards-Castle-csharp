using WizardsCastle.Logic.Data;

namespace WizardsCastle.Logic.Situations
{
    internal class CombatVictorySituation : ISituation
    {
        public ISituation PlayThrough(GameData data, GameTools tools)
        {
            var enemy = tools.EnemyProvider.GetEnemy(data.Map, data.CurrentLocation);
            tools.UI.DisplayMessage($"You have defeated the {enemy.Name}.");

            var reward = tools.Randomizer.RollDie(1000);
            data.Player.GoldPieces += reward;
            tools.UI.DisplayMessage($"You have collected {reward} gold pieces.");

            data.Map.SetLocationInfo(data.CurrentLocation, MapCodes.EmptyRoom);

            return tools.SituationBuilder.LeaveRoom();
        }
    }
}