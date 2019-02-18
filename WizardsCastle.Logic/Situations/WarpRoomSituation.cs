using WizardsCastle.Logic.Data;

namespace WizardsCastle.Logic.Situations
{
    internal class WarpRoomSituation : ISituation
    {
        public ISituation PlayThrough(GameData data, GameTools tools)
        {
            var newLocation = tools.Randomizer.RandomLocation();

            tools.UI.ClearActionLog();
            tools.UI.DisplayMessage(Messages.WarpDescription);
            tools.UI.PromptUserAcknowledgement();

            return tools.SituationBuilder.EnterRoom(newLocation);
        }
    }
}