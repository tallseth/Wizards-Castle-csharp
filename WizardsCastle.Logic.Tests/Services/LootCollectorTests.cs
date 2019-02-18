using NUnit.Framework;
using WizardsCastle.Logic.Data;
using WizardsCastle.Logic.Tests.Helpers;

namespace WizardsCastle.Logic.Tests.Services
{
    [TestFixture]
    public class LootCollectorTests
    {
        private MockGameTools _tools;
        private GameConfig _config;
        private LootCollector _collector;
        private GameData _data;

        [SetUp]
        public void Setup()
        {
            _tools = new MockGameTools();
            _config = GameConfig.Standard;
            _collector = new LootCollector(_tools, _config);

            _data = Any.GameData();
        }

        [Test]
        public void CollectMonsterLootGivesRandomGold()
        {
            var reward = Any.Number();
            _data.Player.GoldPieces = Any.Number();
            var expectedGold = reward + _data.Player.GoldPieces;

            _tools.RandomizerMock.Setup(r => r.RollDie(1000)).Returns(reward);

            var actual = _collector.CollectMonsterLoot(_data);

            Assert.That(actual, Is.EqualTo(string.Format("You have collected {0} gold pieces.", reward)));
            Assert.That(_data.Player.GoldPieces, Is.EqualTo(expectedGold));            
        }

        [Test]
        public void CollectMonsterLootIncrementsMonstersDefeated()
        {
            var original = Any.Number();
            _data.Player.MonstersDefeated = original;

            _collector.CollectMonsterLoot(_data);

            Assert.That(_data.Player.MonstersDefeated, Is.EqualTo(original + 1));
        }

        [Test]
        public void CollectMonsterLootRewardsWithRunestaffIfMonsterHasIt()
        {
            _data.Player.MonstersDefeated = Any.Number();
            _data.Player.HasRuneStaff = false;
            _data.RunestaffDiscovered = false;

            var chances = _config.TotalMonsters - _data.Player.MonstersDefeated; 
            _tools.RandomizerMock.Setup(r => r.OneChanceIn(chances)).Returns(true);

            var reward = Any.Number();
            _tools.RandomizerMock.Setup(r => r.RollDie(1000)).Returns(reward);

            var actual = _collector.CollectMonsterLoot(_data);

            Assert.That(actual, Is.EqualTo(string.Format("You have collected {0} gold pieces.\r\n{1}", reward, Messages.RunestaffAcquired)));
            Assert.That(_data.Player.HasRuneStaff, Is.True);
            Assert.That(_data.RunestaffDiscovered, Is.True);
        }

        [Test]
        public void CollectMonsterLootDoesNotGiveRunestaffIfPlayerPreviouslyAcquiredIt()
        {
            _data.Player.MonstersDefeated = Any.Number();
            _data.Player.HasRuneStaff = false;
            _data.RunestaffDiscovered = true;

            var chances = _config.TotalMonsters - _data.Player.MonstersDefeated; 
            _tools.RandomizerMock.Setup(r => r.OneChanceIn(chances)).Returns(true);

            var reward = Any.Number();
            _tools.RandomizerMock.Setup(r => r.RollDie(1000)).Returns(reward);

            var actual = _collector.CollectMonsterLoot(_data);

            Assert.That(actual, Is.EqualTo(string.Format("You have collected {0} gold pieces.", reward)));
            Assert.That(_data.Player.HasRuneStaff, Is.False);
            Assert.That(_data.RunestaffDiscovered, Is.True);
        }

        [Test]
        public void BoundaryCase()
        {
            _data.Player.MonstersDefeated = _config.TotalMonsters - 1;
            _data.Player.HasRuneStaff = false;
            _data.RunestaffDiscovered = false;

            _collector.CollectMonsterLoot(_data);

            _tools.RandomizerMock.Verify(r => r.OneChanceIn(1));
        }

    }
}