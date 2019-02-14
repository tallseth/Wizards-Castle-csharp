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
            var enemy = Any.Monster();
            var reward = Any.Number();
            _data.Player.GoldPieces = Any.Number();
            var expectedGold = reward + _data.Player.GoldPieces;
            var next = Mock.Of<ISituation>();

            _tools.EnemyProviderMock.Setup(e => e.GetEnemy(_data.Map, _data.CurrentLocation)).Returns(enemy);
            _tools.RandomizerMock.Setup(r => r.RollDie(1000)).Returns(reward);
            _tools.SituationBuilderMock.Setup(sb => sb.LeaveRoom()).Returns(next);

            var actual = _situation.PlayThrough(_data, _tools);

            Assert.That(actual, Is.SameAs(next));
            Assert.That(_data.Player.GoldPieces, Is.EqualTo(expectedGold));
        }
    }
}