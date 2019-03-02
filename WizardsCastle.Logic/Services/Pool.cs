using System;
using System.Collections.Generic;
using System.Text;
using WizardsCastle.Logic.Data;

namespace WizardsCastle.Logic.Services
{
    internal interface IPool
    {
        string DrinkFrom(Player player);
    }

    internal class Pool : IPool
    {
        private readonly IRandomizer _randomizer;

        public Pool(IRandomizer randomizer)
        {
            _randomizer = randomizer;
        }

        public string DrinkFrom(Player player)
        {
            var impact = _randomizer.RollDie(3);

            switch (_randomizer.RollDie(6))
            {
                case 1:
                    player.Strength = Math.Min(18, player.Strength + impact);
                    return Messages.Stronger;
                case 2:
                    player.Strength -= impact;
                    return Messages.Weaker;
                case 3:
                    player.Intelligence = Math.Min(18, player.Intelligence + impact);
                    return Messages.Smarter;
                case 4:
                    player.Intelligence -= impact;
                    return Messages.Dumber;
                case 5:
                    player.Dexterity = Math.Min(18, player.Dexterity + impact);;
                    return Messages.Nimbler;
                case 6:
                    player.Dexterity -= impact;
                    return Messages.Clumsier;
            }

            throw new InvalidOperationException();
        }
    }
}
