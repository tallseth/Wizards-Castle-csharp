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
        private readonly byte _orbFloor;

        public RoomEnumerator(GameConfig config, GameTools tools)
        {
            _config = config;
            _orbFloor = tools.Randomizer.RandomFloor();
        }
        public IEnumerable<string> GetRoomContents(byte floor)
        {
            foreach (var room in GetMonsters())
                yield return room;


            yield return floor == _orbFloor 
                ? MapCodes.WarpOfZot 
                : MapCodes.Warp.ToString();
            yield return MapCodes.Warp.ToString();
            yield return MapCodes.Warp.ToString();


            for (var i = 0; i < 3; i++)
            {
                yield return MapCodes.Sinkhole.ToString();
                yield return MapCodes.Vendor.ToString();
                yield return MapCodes.Gold.ToString();
            }
        }

        private IEnumerable<string> GetMonsters()
        {
            return Enum.GetValues(typeof(Monster)).Cast<int>().Where(i=> i <= _config.MonstersPerFloor).Select(m => $"M{m}");
        }
    }
}