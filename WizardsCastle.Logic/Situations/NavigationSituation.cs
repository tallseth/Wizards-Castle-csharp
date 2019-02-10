using WizardsCastle.Logic.Data;
using WizardsCastle.Logic.UI;

namespace WizardsCastle.Logic.Situations
{
    internal class NavigationSituation : ISituation
    {
        private readonly UserOption[] _movementOptions;

        public NavigationSituation(UserOption[] movementOptions)
        {
            _movementOptions = movementOptions;
        }

        public ISituation PlayThrough(GameData data, GameTools tools)
        {
            var move = tools.UI.PromptUserChoice(_movementOptions).GetData<Move>();

            if (move == Move.Exit)
            {
                var exitMessage = data.Player.HasOrbOfZot
                    ? Messages.WinTheGame
                    : Messages.LostByRetreat;
                return tools.SituationBuilder.GameOver(exitMessage);
            }

            var newLocation = tools.MoveInterpreter.GetTargetLocation(data.CurrentLocation, move);

            return tools.SituationBuilder.EnterRoom(newLocation);
        }
    }
}