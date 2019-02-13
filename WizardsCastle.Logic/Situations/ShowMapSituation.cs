using WizardsCastle.Logic.Data;

namespace WizardsCastle.Logic.Situations
{
    internal class ShowMapSituation : ISituation
    {
        public ISituation PlayThrough(GameData data, GameTools tools)
        {
            tools.UI.DisplayMessage(data.Map.GetDisplayMap(data.CurrentLocation));
            return tools.SituationBuilder.LeaveRoom();
        }
    }
}