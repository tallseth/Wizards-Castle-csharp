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

        [SetUp]
        public void Setup()
        {
            _config = GameConfig.Standard;
            _tools = new MockGameTools();

            _enumerator = new RoomEnumerator(_config, _tools);
        }

        [Test]
        public void EnumeratesMonsters()
        {
            var result = _enumerator.GetRoomContents(1).ToArray();
            var expected = Enumerable.Range(1, 12).Select(i => "M" + i).ToArray();

            Assert.That(result, Is.EqualTo(expected));
        }
    }
}