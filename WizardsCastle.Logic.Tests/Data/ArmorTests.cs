using NUnit.Framework;
using WizardsCastle.Logic.Data;

namespace WizardsCastle.Logic.Tests.Data
{
    [TestFixture]
    internal class ArmorTests
    {
        [Test]
        public void Leather()
        {
            AssertArmorProperties(Armor.Leather, "Leather", 1, 7);
        }

        [Test]
        public void Chainmail()
        {
            AssertArmorProperties(Armor.Chainmail, "Chainmail", 2, 14);
        }

        [Test]
        public void Plate()
        {
            AssertArmorProperties(Armor.Plate, "Plate", 3, 21);
        }

        private static void AssertArmorProperties(Armor armor, string expectedName, int expectedProtection, int expectedDurability)
        {
            Assert.That(armor.Name, Is.EqualTo(expectedName));
            Assert.That(armor.Protection, Is.EqualTo(expectedProtection));
            Assert.That(armor.Durability, Is.EqualTo(expectedDurability));
        }
    }
}