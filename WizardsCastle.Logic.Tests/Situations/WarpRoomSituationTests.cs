using NUnit.Framework;
using WizardsCastle.Logic.Situations;
using WizardsCastle.Logic.Tests.Helpers;

namespace WizardsCastle.Logic.Tests.Situations
{
    [TestFixture]
    public class WarpRoomSituationTests
    {
        [Test]
        public void EntersRandomRoom()
        {
            var tools = new MockGameTools();
            var data = Any.GameData();
            var situation = new WarpRoomSituation();

            var warpTarget = Any.Location();
            tools.RandomizerMock.Setup(r => r.RandomLocation()).Returns(warpTarget);
            var next = tools.SetupNextSituation(sb => sb.EnterRoom(warpTarget));

            Assert.That(situation.PlayThrough(data, tools), Is.SameAs(next));
        }
    }
}