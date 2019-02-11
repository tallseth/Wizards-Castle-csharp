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

            if (roomInfo[0] == MapCodes.UnexploredPrefix)
            {
                roomInfo = roomInfo.TrimStart(MapCodes.UnexploredPrefix);
                data.Map.SetLocationInfo(data.CurrentLocation, roomInfo);
            }

            //todo: occasional spooky message here
            //todo: other turn operations, like curses etc?

            switch (roomInfo[0])
            {
                case MapCodes.Entrance:
                    return SimpleNavigationCase(tools, Messages.EntranceDescription, NavigationOptions.Entrance);
                case MapCodes.EmptyRoom:
                    return SimpleNavigationCase(tools, Messages.EmptyRoomDescription, NavigationOptions.Standard);
            }

            throw new ArgumentException($"Unknown room code: {roomInfo}");
        }

        private static ISituation SimpleNavigationCase(GameTools tools, string roomDescription, UserOption[] moveOptions)
        {
            tools.UI.DisplayMessage(roomDescription);
            return tools.SituationBuilder.Navigate(moveOptions);
        }
    }
}