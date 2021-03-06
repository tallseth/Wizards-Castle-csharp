﻿using WizardsCastle.Logic.Data;
using WizardsCastle.Logic.Purchases;
using WizardsCastle.Logic.Services;
using WizardsCastle.Logic.Situations;
using WizardsCastle.Logic.UI;

namespace WizardsCastle.Logic
{
    internal class GameTools
    {
        internal IGameUI UI { get;set; }
        internal ITeleportUI TeleportUI { get; set; }
        internal IGameDataBuilder DataBuilder { get; set; }
        internal ISituationBuilder SituationBuilder { get; set; }
        internal IMoveInterpreter MoveInterpreter { get; set; }
        internal IRandomizer Randomizer { get; set; }
        internal ICombatService CombatService { get; set; }
        internal IEnemyProvider EnemyProvider { get; set; }
        internal ICurseEvaluator CurseEvaluator { get; set; }
        internal ICombatDice CombatDice { get; set; }
        internal IRoomEnumerator RoomEnumerator { get; set; }
        internal ILootCollector LootCollector { get; set; }
        internal IPurchaseUI PurchaseUI { get; set; }
        internal IInventoryProvider InventoryProvider { get; set; }
        internal IPool Pool { get; set; }

        internal static GameTools Create(GameConfig config)
        {
            var tools = new GameTools
            {
                UI = new GameUI(),
                Randomizer = new Randomizer(config),
                CurseEvaluator = new CurseEvaluator()
            };

            tools.DataBuilder = new GameDataBuilder(config, tools);
            tools.SituationBuilder = new SituationBuilder();
            tools.MoveInterpreter = new MoveInterpreter(config);
            tools.CombatService = new CombatService(tools);
            tools.EnemyProvider = new CachingEnemyProvider(new EnemyProvider());
            tools.CombatDice = new CombatDice(tools.Randomizer);
            tools.RoomEnumerator = new RoomEnumerator(config, tools);
            tools.LootCollector = new LootCollector(tools, config);
            tools.TeleportUI = new TeleportUI(config, tools);
            tools.PurchaseUI = new PurchaseUI(tools.UI);
            tools.InventoryProvider = new InventoryProvider(tools.Randomizer);
            tools.Pool = new Pool(tools.Randomizer);

            return tools;
        }
    }
}