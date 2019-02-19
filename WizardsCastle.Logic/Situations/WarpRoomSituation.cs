using WizardsCastle.Logic.Data;

namespace WizardsCastle.Logic.Situations
{
    internal class WarpRoomSituation : ISituation
    {
        private readonly bool _warpOfZot;

        public WarpRoomSituation(bool warpOfZot)
        {
            _warpOfZot = warpOfZot;
        }

        public ISituation PlayThrough(GameData data, GameTools tools)
        {
            if (_warpOfZot && data.Player.HasRuneStaff && data.LastMove == Move.Teleport)
            {
                tools.UI.ClearActionLog();
                tools.UI.DisplayMessage(Messages.OrbOfZotAcquired);
                tools.UI.PromptUserAcknowledgement();

                data.Player.HasRuneStaff = false;
                data.Player.HasOrbOfZot = true;
                data.Map.SetLocationInfo(data.CurrentLocation, MapCodes.EmptyRoom);

                return tools.SituationBuilder.LeaveRoom();
            }

            var newLocation = _warpOfZot
                ? tools.MoveInterpreter.GetTargetLocation(data.CurrentLocation, data.LastMove) 
                : tools.Randomizer.RandomLocation();

            tools.UI.ClearActionLog();
            tools.UI.DisplayMessage(Messages.WarpDescription);
            tools.UI.PromptUserAcknowledgement();

            return tools.SituationBuilder.EnterRoom(newLocation);
        }
    }
}