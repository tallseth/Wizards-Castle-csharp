using WizardsCastle.Logic.Data;

namespace WizardsCastle.Logic
{
    public class Game
    {
        private readonly GameTools _tools;

        public Game(GameConfig gameConfig) : this (GameTools.Create(gameConfig)){}

        internal Game(GameTools tools)
        {
            _tools = tools;
        }

        public void Play()
        {
            var data = _tools.DataBuilder.CreateGameData();

            var situation = _tools.SituationBuilder.Start();

            while (situation != null) 
                situation = situation.PlayThrough(data, _tools);
        }
    }
}
