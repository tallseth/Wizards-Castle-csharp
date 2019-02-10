using System.Collections.Generic;

namespace WizardsCastle.Logic.Data
{
    internal class Map
    {
        private readonly List<string[,]> _map;

        public Map(GameConfig config)
        {
            _map = new List<string[,]>(config.Floors);
            for (int i = 0; i < config.Floors; i++) 
                _map.Add(new string[config.FloorWidth, config.FloorHeight]);
        }

        public string GetLocationInfo(Location location)
        {
            var val = _map[location.Floor][location.X, location.Y];
            if (val == null)
                return MapCodes.UnexploredPrefix + MapCodes.EmptyRoom;

            return val;
        }

        public void SetLocationInfo(Location location, string info)
        {
            _map[location.Floor][location.X, location.Y] = info;
        }
    }
}