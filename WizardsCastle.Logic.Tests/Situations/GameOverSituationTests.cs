using NUnit.Framework;
using WizardsCastle.Logic.Data;
using WizardsCastle.Logic.Situations;
using WizardsCastle.Logic.Tests.Helpers;

namespace WizardsCastle.Logic.Tests.Situations
{
    [TestFixture]
    public class GameOverSituationTests
    {
        private ISituation _situation;
        private MockGameTools _tools;
        private GameData _data;
        private string _exitMessage;

        [SetUp]
        public void Setup()
        {
            _exitMessage = Any.String();
            _situation = new GameOverSituation(_exitMessage);

            _tools = new MockGameTools();
            _data = Any.GameData();
        }

        [Test]
        public void GameOverHasNoNextSituation()
        {
            Assert.That(_situation.PlayThrough(_data, _tools), Is.Null);
        }

        [Test]
        public void GameOverTellsTheUser()
        {
            var sequence = new MoqSequence();
            sequence.InSequence(_tools.UIMock.Setup(ui => ui.DisplayMessage(_exitMessage)));
            sequence.InSequence(_tools.UIMock.Setup(ui => ui.PromptUserAcknowledgement()));

            _situation.PlayThrough(_data, _tools);

            sequence.Verify();
        }
    }
}