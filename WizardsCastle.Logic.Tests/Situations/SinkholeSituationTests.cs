using NUnit.Framework;
using WizardsCastle.Logic.Data;
using WizardsCastle.Logic.Situations;
using WizardsCastle.Logic.Tests.Helpers;

namespace WizardsCastle.Logic.Tests.Situations
{
    [TestFixture]
    public class SinkholeSituationTests
    {
        private ISituation _situation;
        private MockGameTools _tools;
        private GameData _data;

        [SetUp]
        public void Setup()
        {
            _situation = new SituationBuilder().Sinkhole();

            _tools = new MockGameTools();
            _data = Any.GameData();
        }

        [Test]
        public void FallThroughToNextFloorDown()
        {
            var newLocation = Any.Location();
            _tools.MoveInterpreterMock.Setup(i => i.GetTargetLocation(_data.CurrentLocation, Move.Downstairs)).Returns(newLocation);
            var next = _tools.SetupNextSituation(sb => sb.EnterRoom(newLocation));

            Assert.That(_situation.PlayThrough(_data, _tools), Is.SameAs(next));
        }

        [Test]
        public void InformsUser()
        {
            var sequence = new MoqSequence();
            sequence.InSequence(_tools.UIMock.Setup(ui=>ui.DisplayMessage(Messages.SinkholeDescription)));
            sequence.InSequence(_tools.UIMock.Setup(ui=>ui.PromptUserAcknowledgement()));

            _situation.PlayThrough(_data, _tools);

            sequence.Verify();
        }
    }
}