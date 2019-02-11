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
                case MapCodes.StairsUp:
                    return SimpleNavigationCase(tools, Messages.StairsUp, NavigationOptions.Standard.Add(NavigationOptions.StairsUp));
                case MapCodes.StairsDown:
                    return SimpleNavigationCase(tools, Messages.StairsDown, NavigationOptions.Standard.Add(NavigationOptions.StairsDown));
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