using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using WizardsCastle.Logic.Data;
using WizardsCastle.Logic.Services;
using WizardsCastle.Logic.Tests.Helpers;

namespace WizardsCastle.Logic.Tests.Services
{
    [TestFixture]
    public class GameDataBuilderTests
    {
        private MockGameTools _tools;
        private GameDataBuilder _builder;
        private GameConfig _gameConfig;

        [SetUp]
        public void Setup()
        {
            _tools = new MockGameTools();
            _gameConfig = GameConfig.Standard;
            _builder = new GameDataBuilder(_gameConfig, _tools);

            _tools.DataBuilder = _builder;

            _tools.RandomizerMock.Setup(r => r.Shuffle(It.IsAny<IEnumerable<Location>>()))
                .Returns<IEnumerable<Location>>(input => new Stack<Location>(input));
        }

        [Test]
        public void CounterIsSetToZero()
        {
            var data = _builder.CreateGameData();

            Assert.That(data.TurnCounter, Is.EqualTo(0));
        }

        [Test]
        public void PlayerIsNull()
        {
            var data = _builder.CreateGameData();

            Assert.That(data.Player, Is.Null);
        }

        [Test]
        public void MapIsNotNull()
        {
            var data = _builder.CreateGameData();

            Assert.That(data.Map, Is.Not.Null);
        }

        [Test]
        public void CurrentLocationIsSetToEntrance()
        {
            var data = _builder.CreateGameData();

            Assert.That(data.CurrentLocation, Is.EqualTo(_gameConfig.Entrance));
        }

        [Test]
        public void MapEntranceMarked()
        {
            var data = _builder.CreateGameData();

            Assert.That(data.Map.GetLocationInfo(_gameConfig.Entrance), Is.EqualTo("E"));
        }

        [Test]
        public void StairsArePlacedFirst()
        {
            var data = _builder.CreateGameData();

            AssertLocationValue(data.Map, new Location(7, 7, 0), "?U");
            AssertLocationValue(data.Map, new Location(6, 7, 0), "?U");
            AssertLocationValue(data.Map, new Location(7, 7, 1), "?D");
            AssertLocationValue(data.Map, new Location(6, 7, 1), "?D");
        }

        private void AssertLocationValue(Map map, Location location, string expected)
        {
            Assert.That(map.GetLocationInfo(location), Is.EqualTo(expected));
        }
    }
}
