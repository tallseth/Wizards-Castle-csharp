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

            return map;
        }
    }
}
