using System.Collections.Generic;

namespace WizardsCastle.Logic.Data
{
    internal class Map
    {
        private readonly GameConfig _config;
        private readonly List<string[,]> _map;

        public Map(GameConfig config)
        {
            _config = config;
            _map = new List<string[,]>(config.Floors);
            for (int i = 0; i < config.Floors; i++) 
                _map.Add(new string[config.FloorWidth, config.FloorHeight]);
        }

        public string GetLocationInfo(Location location)
        {
            var val = _map[location.Floor][location.X, location.Y];
            if (val == null)
                return MapCodes.Unexplored(MapCodes.EmptyRoom);

            return val;
        }

        public void SetLocationInfo(Location location, string info)
        {
            _map[location.Floor][location.X, location.Y] = info;
        }

        public void SetLocationInfo(Location location, char code)
        {
            SetLocationInfo(location, code.ToString());
        }

        public IEnumerable<Location> GetEmptyRooms(byte floorNumber)
        {
            var floor = _map[floorNumber];

            for (byte y = 0; y < _config.FloorHeight; y++)
            {
                for (byte x = 0; x < _config.FloorWidth; x++)
                {
                    var roomCode = floor[x, y];
                    if (roomCode == null || roomCode == MapCodes.EmptyRoom.ToString() || roomCode == MapCodes.Unexplored(MapCodes.EmptyRoom))
                        yield return new Location(x, y, floorNumber);
                }
            }
        }
    }
}