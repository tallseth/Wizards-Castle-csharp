using WizardsCastle.Logic.Data;

namespace WizardsCastle.Logic.Services
{
    internal interface ICombatService
    {
        bool PlayerGoesFirst(Player player);
    }

    internal class CombatService : ICombatService
    {
        private readonly GameTools _tools;

        public CombatService(GameTools tools)
        {
            _tools = tools;
        }

        public bool PlayerGoesFirst(Player player)
        {
            return false;
        }
    }
}