using Moq;
using NUnit.Framework;
using WizardsCastle.Logic.Data;
using WizardsCastle.Logic.Situations;
using WizardsCastle.Logic.Tests.Helpers;

namespace WizardsCastle.Logic.Tests.Situations
{
    [TestFixture]
    public class StartSituationTests
    {
        private ISituation _situation;
        private MockGameTools _tools;
        private GameData _data;

        [SetUp]
        public void Setup()
        {
            _situation = new SituationBuilder().Start();

            _tools = new MockGameTools();
            _data = Any.GameData();
        }

        [Test]
        public void IntroducesTheGame()
        {
            var sequence = new MoqSequence();
            sequence.InSequence(_tools.UIMock.Setup(ui => ui.ClearActionLog()));
            sequence.InSequence(_tools.UIMock.Setup(ui => ui.DisplayMessage(It.IsAny<string>())));
            sequence.InSequence(_tools.UIMock.Setup(ui => ui.PromptUserAcknowledgement()));

            _situation.PlayThrough(_data, _tools);

            sequence.Verify();
        }

        [Test]
        public void MovesToPlayerCreation()
        {
            var playerCreation = Mock.Of<ISituation>();
            _tools.SituationBuilderMock.Setup(b => b.CreatePlayer()).Returns(playerCreation);

            var next = _situation.PlayThrough(_data, _tools);

            Assert.That(next, Is.SameAs(playerCreation));
        }
    }
}