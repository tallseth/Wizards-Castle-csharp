﻿using NUnit.Framework;
using WizardsCastle.Logic.Combat;
using WizardsCastle.Logic.Data;

namespace WizardsCastle.Logic.Tests.Combat
{
    [TestFixture]
    internal class EnemyTests
    {
        [TestCase(Monster.Kobold, 3, 1)]
        [TestCase(Monster.Orc, 4, 2)]
        [TestCase(Monster.Wolf, 5, 2)]
        [TestCase(Monster.Goblin, 6, 3)]
        [TestCase(Monster.Ogre, 7, 3)]
        [TestCase(Monster.Troll, 8, 4)]
        [TestCase(Monster.Bear, 9, 4)]
        [TestCase(Monster.Minotaur, 10, 5)]
        [TestCase(Monster.Gargoyle, 11, 5)]
        [TestCase(Monster.Chimera, 12, 6)]
        [TestCase(Monster.Balrog, 13, 6)]
        [TestCase(Monster.Dragon, 14, 7)]
        public void CreateMonsterWithCorrectStats(Monster type, int expectedHP, int expectedDmg)
        {
            var monster = Enemy.CreateMonster(type);

            Assert.That(monster.Name, Is.EqualTo(type.ToString()));
            Assert.That(monster.HitPoints, Is.EqualTo(expectedHP));
            Assert.That(monster.Damage, Is.EqualTo(expectedDmg));
        }

        [Test]
        public void VendorIsPowerfulCombatant()
        {
            var vendor = Enemy.CreateVendorCombatant();

            Assert.That(vendor.Name, Is.EqualTo("Vendor"));
            Assert.That(vendor.HitPoints, Is.EqualTo(15));
            Assert.That(vendor.Damage, Is.EqualTo(7));
        }
    }
}