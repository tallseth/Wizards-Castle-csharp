using System;
using WizardsCastle.Logic.Data;
using WizardsCastle.Logic.UI;

namespace WizardsCastle.Logic.Situations
{
    internal class EnterRoomSituation : ISituation
    {
        private readonly Location _location;

        public EnterRoomSituation(Location location)
        {
            _location = location;
        }

        public ISituation PlayThrough(GameData data, GameTools tools)
        {
            data.TurnCounter++;
            data.CurrentLocation = _location;
            var roomInfo = data.Map.GetLocationInfo(data.CurrentLocation);

            if (roomInfo.StartsWith("?"))
            {
                roomInfo = roomInfo.TrimStart('?');
                data.Map.SetLocationInfo(data.CurrentLocation, roomInfo);
            }

            //todo: occasional spooky message here
            //todo: other turn operations, like curses etc?

            if (roomInfo == "E")
            {
                tools.UI.DisplayMessage(Messages.EntranceDescription);

                return tools.SituationBuilder.Navigate(NavigationOptions.Entrance);
            }

            if (roomInfo == ".")
            {
                tools.UI.DisplayMessage(Messages.EmptyRoomDescription);

                return tools.SituationBuilder.Navigate(NavigationOptions.Standard);
            }

            throw new ArgumentException($"Unknown room code: {roomInfo}");
        }
    }
}