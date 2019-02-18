using System;
using System.Text;
using WizardsCastle.Logic.Data;

namespace WizardsCastle.Logic
{
    internal interface ILootCollector
    {
        string CollectVendorLoot(GameData data);
        string CollectMonsterLoot(GameData data);
    }

    internal class LootCollector : ILootCollector
    {
        private readonly GameTools _tools;
        private readonly GameConfig _config;

        public LootCollector(GameTools tools, GameConfig config)
        {
            _tools = tools;
            _config = config;
        }

        public string CollectVendorLoot(GameData data)
        {
            throw new NotImplementedException();
        }

        public string CollectMonsterLoot(GameData data)
        {
            var player = data.Player;
            var sb = new StringBuilder();

            var reward = _tools.Randomizer.RollDie(1000);
            player.GoldPieces += reward;
            sb.Append($"You have collected {reward} gold pieces.");

            if (!data.RunestaffDiscovered && _tools.Randomizer.OneChanceIn(_config.TotalMonsters - player.MonstersDefeated))
            {
                player.HasRuneStaff = true;
                data.RunestaffDiscovered = true;
                sb.AppendLine().Append(Messages.RunestaffAcquired);
            }

            player.MonstersDefeated++;

            return sb.ToString();
        }
    }
}