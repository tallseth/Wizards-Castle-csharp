using WizardsCastle.Logic.Data;
using WizardsCastle.Logic.UI;

namespace WizardsCastle.Logic.Situations
{
    internal class LeaveRoomSituation : ISituation
    {
        public ISituation PlayThrough(GameData data, GameTools tools)
        {
            var roomInfo = data.Map.GetLocationInfo(data.CurrentLocation);


            switch (roomInfo[0])
            {
                case MapCodes.Entrance:
                    return tools.SituationBuilder.Navigate(NavigationOptions.Entrance);
                case MapCodes.StairsUp:
                    return tools.SituationBuilder.Navigate(NavigationOptions.Standard.Add(NavigationOptions.StairsUp));
                case MapCodes.StairsDown:
                    return tools.SituationBuilder.Navigate(NavigationOptions.Standard.Add(NavigationOptions.StairsDown));
            }

            return tools.SituationBuilder.Navigate(NavigationOptions.Standard);
        }
    }
}