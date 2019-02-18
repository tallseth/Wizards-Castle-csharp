using Moq;
using NUnit.Framework;
using WizardsCastle.Logic.Data;
using WizardsCastle.Logic.Situations;
using WizardsCastle.Logic.Tests.Helpers;

namespace WizardsCastle.Logic.Tests.Situations
{
    [TestFixture]
    public class CombatVictorySituationTests
    {
        private ISituation _situation;
        private MockGameTools _tools;
        private GameData _data;

        [SetUp]
        public void Setup()
        {
            _situation = new CombatVictorySituation();

            _tools = new MockGameTools();
            _data = Any.GameData();
        }

        [Test]
        public void VictorGathersRewardAndLeavesTheRoom()
        {
            var enemy = _tools.SetupEnemyAtCurrentLocation(_data);
            var next = _tools.SetupNextSituation(sb => sb.LeaveRoom());
            var lootMessage = Any.String();
            _tools.LootCollectorMock.Setup(lc => lc.CollectMonsterLoot(_data.Player)).Returns(lootMessage);

            var actual = _situation.PlayThrough(_data, _tools);

            Assert.That(actual, Is.SameAs(next));
            _tools.UIMock.Verify(ui=>ui.DisplayMessage(lootMessage));
        }
    }
}