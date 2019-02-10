using WizardsCastle.Logic.Data;

namespace WizardsCastle.Logic.Situations
{
    internal class StartSituation : ISituation
    {
        public ISituation PlayThrough(GameData data, GameTools tools)
        {
            tools.UI.ClearActionLog();
            tools.UI.DisplayMessage("Welcome to Wizard's Castle!"); //todo: correct start message
            tools.UI.PromptUserAcknowledgement();
            return tools.SituationBuilder.CreatePlayer();
        }
    }
}