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
            tools.UI.ClearActionLog();

            data.TurnCounter++;
            data.CurrentLocation = _location;

            tools.UI.DisplayMessage(data.ToString());

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
                    tools.UI.DisplayMessage(Messages.EntranceDescription);
                    return tools.SituationBuilder.LeaveRoom();
                case MapCodes.EmptyRoom:
                    tools.UI.DisplayMessage(Messages.EmptyRoomDescription);
                    return tools.SituationBuilder.LeaveRoom();
                case MapCodes.StairsUp:
                    tools.UI.DisplayMessage(Messages.StairsUp);
                    return tools.SituationBuilder.LeaveRoom();
                case MapCodes.StairsDown:
                    tools.UI.DisplayMessage(Messages.StairsDown);
                    return tools.SituationBuilder.LeaveRoom();
                case MapCodes.MonsterPrefix:
                    var enemy = tools.EnemyProvider.GetEnemy(data.Map, data.CurrentLocation);
                    tools.UI.ClearActionLog();
                    tools.UI.DisplayMessage($"You have encountered a {enemy.Name}");
                    return tools.SituationBuilder.EnterCombat();
            }

            throw new ArgumentException($"Unknown room code: {roomInfo}");
        }
    }
}