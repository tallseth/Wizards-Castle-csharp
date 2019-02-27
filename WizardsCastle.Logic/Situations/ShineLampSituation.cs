using WizardsCastle.Logic.Data;
using WizardsCastle.Logic.UI;

namespace WizardsCastle.Logic.Situations
{
    internal class ShineLampSituation : ISituation
    {
        public ISituation PlayThrough(GameData data, GameTools tools)
        {
            tools.UI.ClearActionLog();
            tools.UI.DisplayMessage("Which direction do you shine your lamp?");
            var move = tools.UI.PromptUserChoice(NavigationOptions.Standard, true).GetData<Move>();
            var target = tools.MoveInterpreter.GetTargetLocation(data.CurrentLocation, move);
            data.Map.SetLocationInfo(target, data.Map.GetLocationInfo(target).TrimStart('?'));
            return tools.SituationBuilder.ShowMap();
        }
    }
}