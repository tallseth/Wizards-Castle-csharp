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
        private Player _player;

        [SetUp]
        public void Setup()
        {
            _tools = new MockGameTools();
            _config = GameConfig.Standard;
            _collector = new LootCollector(_tools, _config);

            _player = Any.Player();
        }

        [Test]
        public void CollectMonsterLootGivesRandomGold()
        {
            var reward = Any.Number();
            _player.GoldPieces = Any.Number();
            var expectedGold = reward + _player.GoldPieces;

            _tools.RandomizerMock.Setup(r => r.RollDie(1000)).Returns(reward);

            var actual = _collector.CollectMonsterLoot(_player);

            Assert.That(actual, Is.EqualTo(string.Format("You have collected {0} gold pieces.", reward)));
            Assert.That(_player.GoldPieces, Is.EqualTo(expectedGold));            
        }
    }
}