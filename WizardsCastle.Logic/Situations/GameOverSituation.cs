using WizardsCastle.Logic.Data;

namespace WizardsCastle.Logic.Situations
{
    internal class GameOverSituation : ISituation
    {
        private readonly string _exitMessage;

        public GameOverSituation(string exitMessage)
        {
            _exitMessage = exitMessage;
        }

        public ISituation PlayThrough(GameData data, GameTools tools)
        {
            tools.UI.DisplayMessage(_exitMessage);
            tools.UI.PromptUserAcknowledgement();

            return null;
        }
    }
}