using WizardsCastle.Logic.Data;

namespace WizardsCastle.Logic.Situations
{
    internal class SinkholeSituation : ISituation
    {
        public ISituation PlayThrough(GameData data, GameTools tools)
        {
            tools.UI.DisplayMessage(Messages.SinkholeDescription);
            tools.UI.PromptUserAcknowledgement();
            var newLocation = tools.MoveInterpreter.GetTargetLocation(data.CurrentLocation, Move.Downstairs);

            return tools.SituationBuilder.EnterRoom(newLocation);
        }
    }
}