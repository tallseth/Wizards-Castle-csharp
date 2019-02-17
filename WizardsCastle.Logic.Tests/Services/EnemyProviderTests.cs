using System;
using System.Linq.Expressions;
using Moq;
using NUnit.Framework;
using WizardsCastle.Logic.Combat;
using WizardsCastle.Logic.Data;
using WizardsCastle.Logic.Services;

namespace WizardsCastle.Logic.Tests.Services
{
    [TestFixture]
    public class EnemyProviderTests
    {
        private EnemyProvider _provider;
        private Map _map;
        private Location _location;

        [SetUp]
        public void Setup()
        {
            _provider = new EnemyProvider();

            _map = Any.Map();
            _location = Any.Location();
        }

        [Test]
        public void GetsMonsterFromMap()
        {
            var monsterType = Any.EnumValue<Monster>();
            var expected = Enemy.CreateMonster(monsterType);
            _map.SetLocationInfo(_location, "M" + (int)monsterType);

            var actual = _provider.GetEnemy(_map, _location);

            Assert.That(actual.Damage, Is.EqualTo(expected.Damage));
            Assert.That(actual.HitPoints, Is.EqualTo(expected.HitPoints));
            Assert.That(actual.StoneSkin, Is.EqualTo(expected.StoneSkin));
            Assert.That(actual.Name, Is.EqualTo(expected.Name));
        }

        [Test]
        public void GetsVendorCombatantFromMap()
        {
            var expected = Enemy.CreateVendorCombatant();
            _map.SetLocationInfo(_location, "V");

            var actual = _provider.GetEnemy(_map, _location);

            Assert.That(actual.Damage, Is.EqualTo(expected.Damage));
            Assert.That(actual.HitPoints, Is.EqualTo(expected.HitPoints));
            Assert.That(actual.StoneSkin, Is.EqualTo(expected.StoneSkin));
            Assert.That(actual.Name, Is.EqualTo(expected.Name));
        }
    }

    [TestFixture]
    public class CachingEnemyProviderTests
    {
        private IEnemyProvider _provider;
        private Map _map;
        private Location _location;
        private Mock<IEnemyProvider> _core;

        [SetUp]
        public void Setup()
        {
            _core = new Mock<IEnemyProvider>();
            _provider = new CachingEnemyProvider(_core.Object);

            _map = Any.Map();
            _location = Any.Location();
        }

        [Test]
        public void GetsFromUnderlying()
        {
            var enemy = Any.Monster();
            _core.Setup(c => c.GetEnemy(_map, _location)).Returns(enemy);

            Assert.That(_provider.GetEnemy(_map, _location), Is.SameAs(enemy));
        }

        [Test]
        public void GetsExactSameEnemyOnRepeatedCalls()
        {
            _core.Setup(c => c.GetEnemy(_map, _location)).Returns(Any.Monster);

            var first = _provider.GetEnemy(_map, _location);
            var second = _provider.GetEnemy(_map, _location);

            Assert.That(first, Is.SameAs(second));
            _core.Verify(c=>c.GetEnemy(It.IsAny<Map>(), It.IsAny<Location>()), Times.Once());
        }
    }
}