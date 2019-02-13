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
            return null;
        }
    }
}