using WizardsCastle.Logic.Combat;
using WizardsCastle.Logic.Data;

namespace WizardsCastle.Logic.Services
{
    internal interface ICombatDice
    {
        bool RollToGoFirst(Player player);
        bool RollToHit(Player player);
        bool RollToDodge(Player player);
        bool RollForWeaponBreakage(Enemy enemy);
    }

    internal class CombatDice : ICombatDice
    {
        private readonly IRandomizer _randomizer;

        public CombatDice(IRandomizer randomizer)
        {
            _randomizer = randomizer;
        }

        public bool RollToGoFirst(Player player)
        {
            return !player.IsBlind && player.Dexterity >= _randomizer.RollDice(2, 9);
        }

        public bool RollToHit(Player player)
        {
            // Changed from 1d20 to 1d15 for better hit chance
            return player.Dexterity >= _randomizer.RollDie(15) + GetBlindnessOffset(player);
        }

        public bool RollToDodge(Player player)
        {
            return player.Dexterity >= _randomizer.RollDice(3,7) + GetBlindnessOffset(player);
        }

        public bool RollForWeaponBreakage(Enemy enemy)
        {
            // Reduced from 1/8 to 1/12 for less frequent breakage
            return enemy.StoneSkin && _randomizer.OneChanceIn(12);
        }

        private static int GetBlindnessOffset(Player player)
        {
            // Reduced from +3 to +2 for less punishing blindness
            return player.IsBlind ? 2 : 0;
        }
    }
}