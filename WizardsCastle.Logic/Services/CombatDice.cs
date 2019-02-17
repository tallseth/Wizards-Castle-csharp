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
            return player.Dexterity >= _randomizer.RollDie(20) + GetBlindnessOffset(player);
        }

        public bool RollToDodge(Player player)
        {
            return player.Dexterity >= _randomizer.RollDice(3,7) + GetBlindnessOffset(player);
        }

        public bool RollForWeaponBreakage(Enemy enemy)
        {
            return enemy.StoneSkin && _randomizer.OneChanceIn(8);
        }

        private static int GetBlindnessOffset(Player player)
        {
            return player.IsBlind ? 3 : 0;
        }
    }
}