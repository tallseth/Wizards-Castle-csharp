using System;
using System.Collections.Concurrent;
using System.Linq;
using WizardsCastle.Logic.Combat;
using WizardsCastle.Logic.Data;

namespace WizardsCastle.Logic.Services
{
    internal interface IEnemyProvider
    {
        Enemy GetEnemy(Map map, Location location);
    }

    internal class EnemyProvider : IEnemyProvider
    {
        public Enemy GetEnemy(Map map, Location location)
        {
            var info = map.GetLocationInfo(location);

            if(info.First() == MapCodes.Vendor)
                return Enemy.CreateVendorCombatant();

            if(info.First() != MapCodes.MonsterPrefix)
                throw new InvalidOperationException($"No enemy at {location}.  Actual value: {info}");

            var type = (Monster) Convert.ToInt32(info.Substring(1));

            return Enemy.CreateMonster(type);
        }
    }

    internal class CachingEnemyProvider : IEnemyProvider
    {
        private readonly IEnemyProvider _core;
        private readonly ConcurrentDictionary<Location,Enemy> _cache = new ConcurrentDictionary<Location, Enemy>();

        public CachingEnemyProvider(IEnemyProvider core)
        {
            _core = core;
        }

        public Enemy GetEnemy(Map map, Location location)
        {
            return _cache.GetOrAdd(location, _ => _core.GetEnemy(map, location));
        }
    }
}