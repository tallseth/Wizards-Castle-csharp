using System;
using System.Linq;
using NUnit.Framework;
using WizardsCastle.Logic.Data;
using WizardsCastle.Logic.Tests.Helpers;

namespace WizardsCastle.Logic.Tests.Services
{
    [TestFixture]
    public class RoomEnumeratorTests
    {
        private GameConfig _config;
        private MockGameTools _tools;
        private RoomEnumerator _enumerator;
        private byte _standardFloor;
        private byte _zotFloor;

        [SetUp]
        public void Setup()
        {
            _config = GameConfig.Standard;
            _tools = new MockGameTools();

            _standardFloor = 1;
            _zotFloor = 2;

            _tools.RandomizerMock.SetupSequence(r => r.RandomFloor())
                .Returns(_zotFloor);

            _enumerator = new RoomEnumerator(_config, _tools);
        }

        [Test]
        public void EnumeratesMonsters()
        {
            
            var result = _enumerator.GetRoomContents(1).Where(r=>r.StartsWith(MapCodes.MonsterPrefix)).ToArray();
            var expected = Enumerable.Range(_standardFloor, 12).Select(i => "M" + i).ToArray();

            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void EnumeratesWarp()
        {
            var result = _enumerator.GetRoomContents(_standardFloor).Count(r => r == MapCodes.Warp.ToString());

            Assert.That(result, Is.EqualTo(3));
        }

        [Test]
        public void EnumeratesWarpsForZotFloor()
        {
            var floorContents = _enumerator.GetRoomContents(_zotFloor).ToList();
            var warpCount = floorContents.Count(r => r == MapCodes.Warp.ToString());
            var warpOfZotCount = floorContents.Count(r => r == MapCodes.WarpOfZot);

            Assert.That(warpCount, Is.EqualTo(2));
            Assert.That(warpOfZotCount, Is.EqualTo(1));
        }

        [Test]
        public void EnumeratesSinkhole()
        {
            var result = _enumerator.GetRoomContents(_standardFloor).Count(r => r == MapCodes.Sinkhole.ToString());

            Assert.That(result, Is.EqualTo(3));
        }
    }
}