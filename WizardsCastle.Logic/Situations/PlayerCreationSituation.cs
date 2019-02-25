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
            var chosenRace = tools.UI.PromptUserChoice(RaceOptions.All, true).GetData<Race>();

            data.Player = new Player(chosenRace);
            data.Player.Weapon = Weapon.Mace;
            data.Player.Armor = Armor.Chainmail;
            tools.UI.DisplayMessage(data.Player.ToString());
            
            return tools.SituationBuilder.AllocateStats();
        }
    }
}