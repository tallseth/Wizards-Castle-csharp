using System;
using System.Collections.Generic;
using System.Linq;
using WizardsCastle.Logic.Data;

namespace WizardsCastle.Logic.Services
{
    internal interface IRandomizer
    {
        Stack<T> Shuffle<T>(IEnumerable<T> toShuffle);
        bool OneChanceIn(int n);
        int RollDice(int numDice, int numSides);
        int RollDie(int numSides);
        Location RandomLocation();
    }

    internal class Randomizer : IRandomizer
    {
        private readonly GameConfig _config;
        private static readonly Random _random = new Random();

        public Randomizer(GameConfig config)
        {
            _config = config;
        }

        public Stack<T> Shuffle<T>(IEnumerable<T> toShuffle)
        {
            return new Stack<T>(toShuffle.OrderBy(e=>_random.NextDouble()));
        }

        public bool OneChanceIn(int n)
        {
            return _random.Next(0, n) == 0;
        }

        public int RollDice(int numDice, int numSides)
        {
            var sum = 0;
            for (int i = 0; i < numDice; i++) 
                sum += RollDie(numSides);

            return sum;
        }

        public int RollDie(int numSides)
        {
            return _random.Next(1, numSides + 1);
        }

        public Location RandomLocation()
        {
            return new Location(RandomByte(_config.FloorWidth), RandomByte(_config.FloorHeight), RandomByte(_config.Floors));
        }

        private byte RandomByte(byte max)
        {
            return (byte) _random.Next(max);
        }
    }
}