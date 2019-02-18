using WizardsCastle.Logic.Data;

namespace WizardsCastle.Logic.Situations
{
    internal class TeleportSituation : ISituation
    {
        public ISituation PlayThrough(GameData data, GameTools tools)
        {
            tools.UI.ClearActionLog();
            tools.UI.DisplayMessage("Imagine teleporting now... not implemented");
            tools.UI.PromptUserAcknowledgement();
            return tools.SituationBuilder.EnterRoom(data.CurrentLocation);
        }
    }
}