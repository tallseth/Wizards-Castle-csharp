using NUnit.Framework;
using WizardsCastle.Logic.Data;

namespace WizardsCastle.Logic.Tests.Data
{
    [TestFixture]
    internal class WeaponTests
    {
        [Test]
        public void Dagger()
        {
            AssertWeaponProperties(Weapon.Dagger, "Dagger", 1);
        }

        [Test]
        public void Mace()
        {
            AssertWeaponProperties(Weapon.Mace, "Mace", 2);
        }

        [Test]
        public void Sword()
        {
            AssertWeaponProperties(Weapon.Sword, "Sword", 3);
        }

        private static void AssertWeaponProperties(Weapon weapon, string expectedName, int expectedDamage)
        {
            Assert.That(weapon.Name, Is.EqualTo(expectedName));
            Assert.That(weapon.Damage, Is.EqualTo(expectedDamage));
        }
    }
}