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

        internal static GameTools Create(GameConfig gameConfig)
        {
            var gameTools = new GameTools
            {
                UI = new GameUI(),
                Randomizer = new Randomizer()
            };

            gameTools.DataBuilder = new GameDataBuilder(gameConfig, gameTools);
            gameTools.SituationBuilder = new SituationBuilder();
            gameTools.MoveInterpreter = new MoveInterpreter(gameConfig);
            gameTools.CombatService = new CombatService(gameTools);
            gameTools.EnemyProvider = new EnemyProvider();

            return gameTools;
        }
    }

}