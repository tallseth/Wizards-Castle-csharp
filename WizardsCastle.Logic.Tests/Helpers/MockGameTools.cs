using Moq;
using WizardsCastle.Logic.Services;
using WizardsCastle.Logic.Situations;
using WizardsCastle.Logic.UI;

namespace WizardsCastle.Logic.Tests.Helpers
{
    internal class MockGameTools : GameTools
    {
        public Mock<IGameUI> UIMock { get; }
        public Mock<IGameDataBuilder> GameDataBuilderMock { get; }
        public Mock<ISituationBuilder> SituationBuilderMock { get; }
        public Mock<IMoveInterpreter> MoveInterpreterMock { get; }

        public MockGameTools()
        {
            UIMock = new Mock<IGameUI>();
            UI = UIMock.Object;

            GameDataBuilderMock = new Mock<IGameDataBuilder>();
            DataBuilder = GameDataBuilderMock.Object;

            SituationBuilderMock = new Mock<ISituationBuilder>();
            SituationBuilder = SituationBuilderMock.Object;

            MoveInterpreterMock = new Mock<IMoveInterpreter>();
            MoveInterpreter = MoveInterpreterMock.Object;
        }
    }
}