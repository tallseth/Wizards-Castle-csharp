using WizardsCastle.Logic.Combat;
using WizardsCastle.Logic.Data;

namespace WizardsCastle.Logic.Services
{
    internal interface ICombatService
    {
        bool PlayerGoesFirst(Player player);
        CombatResult PlayerAttacks(Player player, Enemy enemy);
        CombatResult EnemyAttacks(Player player, Enemy enemy);
    }

    internal class CombatResult
    {
        internal bool AttackerMissed { get; set; }
        internal bool DefenderDied { get; set; }
        internal int DamageToDefender { get; set; }
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
            if (player.IsBlind || _tools.CurseEvaluator.IsEffectedByCurse(player, Curses.CurseOfLethargy))
                return false;

            return player.Dexterity >= _tools.Randomizer.RollDice(2, 9);
        }

        public CombatResult PlayerAttacks(Player player, Enemy enemy)
        {
            return new CombatResult();
        }

        public CombatResult EnemyAttacks(Player player, Enemy enemy)
        {
            return new CombatResult();
        }
    }
}