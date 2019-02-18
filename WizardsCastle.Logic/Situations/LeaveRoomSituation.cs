using WizardsCastle.Logic.Data;
using WizardsCastle.Logic.UI;

namespace WizardsCastle.Logic.Situations
{
    internal class LeaveRoomSituation : ISituation
    {
        public ISituation PlayThrough(GameData data, GameTools tools)
        {
            var roomInfo = data.Map.GetLocationInfo(data.CurrentLocation);

            var navOptions = GetNavigationOptions(roomInfo);
            if (data.Player.HasRuneStaff)
                navOptions = navOptions.Add(NavigationOptions.Teleport);

            return tools.SituationBuilder.Navigate(navOptions);
        }

        private UserOption[] GetNavigationOptions(string roomInfo)
        {
            var options = NavigationOptions.Standard;
            switch (roomInfo[0])
            {
                case MapCodes.Entrance:
                    options = NavigationOptions.Entrance;
                    break;
                case MapCodes.StairsUp:
                    options = options.Add(NavigationOptions.StairsUp);
                    break;
                case MapCodes.StairsDown:
                    options = options.Add(NavigationOptions.StairsDown);
                    break;
            }

            return options;
        }
    }
}