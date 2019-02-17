using System.Collections.Generic;
using System.IO;
using WizardsCastle.Logic.Combat;
using WizardsCastle.Logic.Data;
using WizardsCastle.Logic.Services;
using WizardsCastle.Logic.Situations;
using WizardsCastle.Logic.UI;

namespace WizardsCastle.Logic
{
    internal class GameTools
    {
        internal IGameUI UI { get;set; }
        internal IGameDataBuilder DataBuilder { get; set; }
        internal ISituationBuilder SituationBuilder { get; set; }
        internal IMoveInterpreter MoveInterpreter { get; set; }
        internal IRandomizer Randomizer { get; set; }
        internal ICombatService CombatService { get; set; }
        internal IEnemyProvider EnemyProvider { get; set; }
        internal ICurseEvaluator CurseEvaluator { get; set; }
        internal ICombatDice CombatDice { get; set; }
        internal IRoomEnumerator RoomEnumerator { get; set; }

        internal static GameTools Create(GameConfig gameConfig)
        {
            var gameTools = new GameTools
            {
                UI = new GameUI(),
                Randomizer = new Randomizer(gameConfig),
                CurseEvaluator = new CurseEvaluator()
            };

            gameTools.DataBuilder = new GameDataBuilder(gameConfig, gameTools);
            gameTools.SituationBuilder = new SituationBuilder();
            gameTools.MoveInterpreter = new MoveInterpreter(gameConfig);
            gameTools.CombatService = new CombatService(gameTools);
            gameTools.EnemyProvider = new CachingEnemyProvider(new EnemyProvider());
            gameTools.CombatDice = new CombatDice(gameTools.Randomizer);
            gameTools.RoomEnumerator = new RoomEnumerator(gameConfig, gameTools);

            return gameTools;
        }
    }
}