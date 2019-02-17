using WizardsCastle.Logic.Data;

namespace WizardsCastle.Logic.Services
{
    internal interface ICurseEvaluator
    {
        bool IsEffectedByCurse(Player player, Curses curse);
    }

    internal class CurseEvaluator : ICurseEvaluator
    {
        public bool IsEffectedByCurse(Player player, Curses curse)
        {
            //todo: some treasures counter curses
            if (player.HasOrbOfZot || player.HasRuneStaff)
                return false;

            return (player.Curses & curse) == curse;
        }
    }
}