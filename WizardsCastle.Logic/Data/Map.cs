using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public string GetDisplayMap(Location currentLocation)
        {
            const string currentLocationFormat = "<{0}>";
            const string locationFormat = " {0} ";
            const string separator = "  ";

            var sb = new StringBuilder();
            for (byte y = 0; y < _config.FloorHeight; y++)
            {
                for (byte x = 0; x < _config.FloorWidth; x++)
                {
                    var location = new Location(x,y,currentLocation.Floor);
                    var format = location.Equals(currentLocation) ? currentLocationFormat : locationFormat;
                    sb.AppendFormat(format, GetLocationInfo(location).First()).Append(separator);
                }

                sb.AppendLine();
            }
            return sb.ToString();
        }
    }
}