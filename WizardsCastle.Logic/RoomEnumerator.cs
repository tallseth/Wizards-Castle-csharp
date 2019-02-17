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
            return Enum.GetValues(typeof(Monster)).Cast<int>().Where(i=> i <= _config.MonstersPerFloor).Select(m => $"M{m}");
        }
    }
}