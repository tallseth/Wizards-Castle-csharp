using System;
using System.Collections.Generic;
using System.Linq;
using WizardsCastle.Logic.Data;

namespace WizardsCastle.Logic
{
    internal interface IRoomEnumerator
    {
        IEnumerable<string> GetRoomContents(byte floor);
    }

    internal class RoomEnumerator : IRoomEnumerator
    {
        private readonly GameConfig _config;
        private readonly GameTools _tools;

        public RoomEnumerator(GameConfig config, GameTools tools)
        {
            _config = config;
            _tools = tools;
        }
        public IEnumerable<string> GetRoomContents(byte floor)
        {
            foreach (var room in GetMonsters())
                yield return room;

            yield return MapCodes.Warp.ToString();
        }

        private IEnumerable<string> GetMonsters()
        {
            return Enum.GetValues(typeof(Monster)).Cast<int>().Where(i=> i <= _config.MonstersPerFloor).Select(m => $"M{m}");
        }
    }
}