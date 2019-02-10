using WizardsCastle.Logic.Data;
using WizardsCastle.Logic.UI;

namespace WizardsCastle.Logic.Situations
{
    internal class PlayerCreationSituation : ISituation
    {
        public ISituation PlayThrough(GameData data, GameTools tools)
        {
            //todo: real implementation of this
            tools.UI.ClearActionLog();
            tools.UI.DisplayMessage("Choose a Race");
            var chosenRace = tools.UI.PromptUserChoice(RaceOptions.All);

            data.Player = new Player(chosenRace.GetData<Race>());
            tools.UI.DisplayMessage(data.Player.ToString());

            return tools.SituationBuilder.EnterRoom(data.CurrentLocation);
        }
    }
}