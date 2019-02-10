using Moq;
using NUnit.Framework;
using WizardsCastle.Logic.Data;
using WizardsCastle.Logic.Situations;
using WizardsCastle.Logic.Tests.Helpers;

namespace WizardsCastle.Logic.Tests
{
    [TestFixture]
    public class GameTests
    {
        private MockGameTools _tools;
        private Game _game;

        [SetUp]
        public void Setup()
        {
            _tools = new MockGameTools();
            _game = new Game(_tools);
        }

        [Test]
        public void PlayBuildsDataAndRunsSituationsUntilComplete()
        {
            var data = new GameData();
            _tools.GameDataBuilderMock.Setup(b => b.CreateGameData()).Returns(data);

            var situation = new Mock<ISituation>();
            _tools.SituationBuilderMock.Setup(b => b.Start()).Returns(situation.Object);
            situation.SetupSequence(s => s.PlayThrough(data, _tools))
                .Returns(situation.Object)
                .Returns(situation.Object)
                .Returns((ISituation) null);

            _game.Play();

            situation.Verify(s=>s.PlayThrough(data, _tools), Times.Exactly(3));
        }
    }
}
