using System.Collections.Generic;
using WizardsCastle.Logic.Data;

namespace WizardsCastle.Logic.Services
{
    internal interface IGameDataBuilder
    {
        GameData CreateGameData();
    }

    internal class GameDataBuilder : IGameDataBuilder
    {
        private readonly GameConfig _config;
        private readonly GameTools _tools;

        public GameDataBuilder(GameConfig config, GameTools tools)
        {
            _config = config;
            _tools = tools;
        }

        public GameData CreateGameData()
        {
            return new GameData
            {
                TurnCounter = 0,
                Map = CreateMap(),
                CurrentLocation = _config.Entrance
            };
        }

        private Map CreateMap()
        {
            var map = new Map(_config);
            
            map.SetLocationInfo(_config.Entrance, "E");

            for (byte floorNum = 0; floorNum < _config.Floors; floorNum++)
            {
                var currentFloorShuffled = _tools.Randomizer.Shuffle(map.GetEmptyRooms(floorNum));
                var nextFloorNum = (byte)(floorNum + 1);
                if (nextFloorNum < _config.Floors)
                {
                    for (int i = 0; i < _config.StairsPerFloor; i++)
                    {
                        var bottom = currentFloorShuffled.Pop();
                        var top = new Location(bottom.X, bottom.Y, nextFloorNum);
                        map.SetLocationInfo(bottom, MapCodes.Unexplored(MapCodes.StairsUp));
                        map.SetLocationInfo(top, MapCodes.Unexplored(MapCodes.StairsDown));
                    }
                }
            }

            return map;
        }
    }

    internal interface IFloorContentGenerator
    {
        IEnumerable<string> GetContentsForFloor(GameConfig config);
    }

    internal class FloorContentGenerator : IFloorContentGenerator
    {
        public IEnumerable<string> GetContentsForFloor(GameConfig config)
        {
            yield break;
        }
    }
}
