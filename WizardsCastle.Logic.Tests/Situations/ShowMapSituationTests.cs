using Moq;
using NUnit.Framework;
using WizardsCastle.Logic.Situations;
using WizardsCastle.Logic.Tests.Helpers;

namespace WizardsCastle.Logic.Tests.Situations
{
    [TestFixture]
    public class ShowMapSituationTests
    {
        [Test]
        public void ShowsMapAndLeavesRoom()
        {
            var data = Any.GameData();
            var tools = new MockGameTools();
            var situation = new ShowMapSituation();

            var sequence = new MoqSequence();
            sequence.InSequence(tools.UIMock.Setup(ui => ui.ClearActionLog()));
            sequence.InSequence(tools.UIMock.Setup(ui => ui.DisplayMessage(data.Map.GetDisplayMap(data.CurrentLocation))));
            var next = Mock.Of<ISituation>();
            tools.SituationBuilderMock.Setup(b => b.LeaveRoom()).Returns(next);

            Assert.That(situation.PlayThrough(data, tools), Is.SameAs(next));
            sequence.Verify();
        }
    }
}