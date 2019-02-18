using System;
using WizardsCastle.Logic.Data;

namespace WizardsCastle.Logic
{
    internal interface ILootCollector
    {
        string CollectVendorLoot(Player player);
        string CollectMonsterLoot(Player player);
    }

    internal class LootCollector : ILootCollector
    {
        private readonly GameTools _tools;
        private readonly GameConfig _config;
        private int _monstersDefeated;

        public LootCollector(GameTools tools, GameConfig config)
        {
            _tools = tools;
            _config = config;
        }

        public string CollectVendorLoot(Player player)
        {
            throw new NotImplementedException();
        }

        public string CollectMonsterLoot(Player player)
        {
            _monstersDefeated++;
            var reward = _tools.Randomizer.RollDie(1000);
            player.GoldPieces += reward;
            return $"You have collected {reward} gold pieces.";
        }
    }
}