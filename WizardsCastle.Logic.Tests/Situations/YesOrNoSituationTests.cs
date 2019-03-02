using Moq;
using NUnit.Framework;
using WizardsCastle.Logic.Data;
using WizardsCastle.Logic.Situations;
using WizardsCastle.Logic.Tests.Helpers;
using WizardsCastle.Logic.UI;

namespace WizardsCastle.Logic.Tests.Situations
{
    [TestFixture]
    public class YesOrNoSituationTests
    {
        private ISituation _situation;
        private MockGameTools _tools;
        private GameData _data;
        private string _message;
        private ISituation _yesSituation;

        [SetUp]
        public void Setup()
        {
            _message = Any.String();
            _yesSituation = Mock.Of<ISituation>();
            _situation = new SituationBuilder().YesOrNo(_message, _yesSituation);

            _tools = new MockGameTools();
            _data = Any.GameData();
        }

        [Test]
        public void DisplaysMessageThenPromptsUserChoice()
        {
            var sequence = new MoqSequence();
            sequence.InSequence(_tools.UIMock.Setup(ui=>ui.ClearActionLog()));
            sequence.InSequence(_tools.UIMock.Setup(ui=>ui.DisplayMessage(_message)));
            sequence.InSequence(_tools.UIMock.Setup(ui=>ui.PromptUserChoice(YesNoOptions.All, true))).Returns(Any.Of(YesNoOptions.All));

            _situation.PlayThrough(_data, _tools);

            sequence.Verify();
        }

        [Test]
        public void ReturnsYesSituationIfPlayerChoosesYes()
        {
            _tools.UIMock.Setup(ui => ui.PromptUserChoice(YesNoOptions.All, true)).Returns(YesNoOptions.Yes);

            var actual = _situation.PlayThrough(_data, _tools);

            Assert.That(actual, Is.SameAs(_yesSituation));
        }

        [Test]
        public void LeaveRoomIfPlayerChoosesNo()
        {
            _tools.UIMock.Setup(ui => ui.PromptUserChoice(YesNoOptions.All, true)).Returns(YesNoOptions.No);
            var next = _tools.SetupNextSituation(sb => sb.LeaveRoom());

            var actual = _situation.PlayThrough(_data, _tools);

            Assert.That(actual, Is.SameAs(next));
        }
    }
}