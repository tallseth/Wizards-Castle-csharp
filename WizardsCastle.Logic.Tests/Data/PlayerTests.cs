using NUnit.Framework;
using WizardsCastle.Logic.Data;

namespace WizardsCastle.Logic.Tests.Data
{
    [TestFixture]
    internal class PlayerTests
    {
        [TestCase(Race.Dwarf, 10, 6, 8)]
        [TestCase(Race.Human, 8, 8, 8)]
        [TestCase(Race.Elf, 6, 10, 8)]
        [TestCase(Race.Hobbit, 4, 12, 4)]
        public void PlayerCreation(Race race, int expectedStr, int expectedDex, int expectedUnallocated)
        {
            var player = new Player(race);

            Assert.That(player.Race, Is.EqualTo(race));
            Assert.That(player.Strength, Is.EqualTo(expectedStr));
            Assert.That(player.Dexterity, Is.EqualTo(expectedDex));
            Assert.That(player.UnallocatedStats, Is.EqualTo(expectedUnallocated));
        }

        [TestCase(Race.Dwarf)]
        [TestCase(Race.Human)]
        [TestCase(Race.Elf)]
        [TestCase(Race.Hobbit)]
        public void PlayersCreatedWithSetIntelligenceAndGold(Race race)
        {
            var player = new Player(race);

            Assert.That(player.Intelligence, Is.EqualTo(8));
            Assert.That(player.GoldPieces, Is.EqualTo(60));
        }

        [Test]
        public void ToStringGivesStats()
        {
            var player = Any.Player();

            Assert.That(player.ToString().Trim(), Does.StartWith("ST: " + player.Strength + " DE: " + player.Dexterity + " IQ: " + player.Intelligence + " GP: " + player.GoldPieces));
        }
    }
}