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

            switch (roomInfo[0])
            {
                case MapCodes.Entrance:
                    return tools.SituationBuilder.Navigate(NavigationOptions.Entrance);
                case MapCodes.StairsUp:
                    return tools.SituationBuilder.Navigate(NavigationOptions.Standard.Add(NavigationOptions.StairsUp));
                case MapCodes.StairsDown:
                    return tools.SituationBuilder.Navigate(NavigationOptions.Standard.Add(NavigationOptions.StairsDown));
            }

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
                    options = options.Add(NavigationOptions.StairsUp);
                    break;
            }

            return options;
        }
    }
}