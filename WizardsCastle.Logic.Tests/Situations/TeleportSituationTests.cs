using NUnit.Framework;
using WizardsCastle.Logic.Data;
using WizardsCastle.Logic.Situations;
using WizardsCastle.Logic.Tests.Helpers;

namespace WizardsCastle.Logic.Tests.Situations
{
    [TestFixture]
    public class TeleportSituationTests
    {
        private ISituation _situation;
        private MockGameTools _tools;
        private GameData _data;

        [SetUp]
        public void Setup()
        {
            _situation = new SituationBuilder().Teleport();

            _tools = new MockGameTools();
            _data = Any.GameData();
        }

        [Test]
        public void CollectsCoordinatesAndEntersRoom()
        {
            var target = Any.Location();
            _tools.TeleportUIMock.Setup(ui => ui.GetTeleportationTarget()).Returns(target);
            var next = _tools.SetupNextSituation(sb => sb.EnterRoom(target));

            var actual = _situation.PlayThrough(_data, _tools);

            Assert.That(actual, Is.EqualTo(next));
        }
    }
}