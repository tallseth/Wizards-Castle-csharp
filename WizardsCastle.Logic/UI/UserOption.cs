using System;
using WizardsCastle.Logic.Data;

namespace WizardsCastle.Logic.UI
{
    internal class UserOption
    {
        private readonly object _data;

        public UserOption(string name, ConsoleKey key, object data)
        {
            _data = data;
            Name = name;
            Key = key;
        }

        internal string Name { get; }
        internal ConsoleKey Key { get; }

        internal T GetData<T>()
        {
            return (T) _data;
        }

        public override string ToString()
        {
            return Name;
        }
    }

    internal static class RaceOptions
    {
        public static readonly UserOption Dwarf = new UserOption("(D)warf", ConsoleKey.D, Race.Dwarf);
        public static readonly UserOption Human = new UserOption("Hu(m)an", ConsoleKey.M, Race.Human);
        public static readonly UserOption Elf = new UserOption("(E)lf", ConsoleKey.E, Race.Elf);
        public static readonly UserOption Halfling = new UserOption("(H)alfling", ConsoleKey.H, Race.Hobbit);

        public static UserOption[] All => new[] {Dwarf, Elf, Human, Halfling};
    }

    internal static class NavigationOptions
    {
        public static readonly UserOption Up = new UserOption("Up", ConsoleKey.UpArrow, Move.Up);
        public static readonly UserOption Down = new UserOption("Down", ConsoleKey.DownArrow, Move.Down);
        public static readonly UserOption Left = new UserOption("Left", ConsoleKey.LeftArrow, Move.Left);
        public static readonly UserOption Right = new UserOption("Right", ConsoleKey.RightArrow, Move.Right);
        public static readonly UserOption Exit = new UserOption("Up", ConsoleKey.UpArrow, Move.Exit);
        public static readonly UserOption StairsDown = new UserOption("Stairs (D)own", ConsoleKey.D, Move.Downstairs);
        public static readonly UserOption StairsUp = new UserOption("Stairs (U)p", ConsoleKey.U, Move.Upstairs);
        public static readonly UserOption Map = new UserOption("Show (M)ap", ConsoleKey.M, Move.ShowMap);
        public static readonly UserOption Teleport = new UserOption("(T)eleport", ConsoleKey.T, Move.Teleport);
        public static readonly UserOption ShineLamp = new UserOption("Shine (L)amp", ConsoleKey.L, Move.ShineLamp);

        public static UserOption[] Standard => new[] {Up, Down, Left, Right};
        public static UserOption[] Entrance => new[] {Exit, Down, Left, Right};
    }

    internal static class CombatOptions
    {
        public static readonly UserOption Attack = new UserOption("(A)ttack", ConsoleKey.A, 'A');
        public static readonly UserOption Retreat = new UserOption("(R)etreat", ConsoleKey.R, 'R');

        public static UserOption[] All => new[] {Attack, Retreat};
    }

    internal static class StatsOptions
    {
        public static readonly UserOption Strength = new UserOption("(S)trength", ConsoleKey.S, 'S');
        public static readonly UserOption Dexterity = new UserOption("(D)exterity", ConsoleKey.D, 'D');
        public static readonly UserOption Intelligence = new UserOption("(I)ntelligence", ConsoleKey.I, 'I');

        public static UserOption[] All => new[] {Strength, Dexterity, Intelligence};
    }

    internal static class NumberOptions
    {
        public static readonly UserOption Zero = new UserOption("0", ConsoleKey.D0, 0);
        public static readonly UserOption One = new UserOption("1", ConsoleKey.D1, 1);
        public static readonly UserOption Two = new UserOption("2", ConsoleKey.D2, 2);
        public static readonly UserOption Three = new UserOption("3", ConsoleKey.D3, 3);
        public static readonly UserOption Four = new UserOption("4", ConsoleKey.D4, 4);
        public static readonly UserOption Five = new UserOption("5", ConsoleKey.D5, 5);
        public static readonly UserOption Six = new UserOption("6", ConsoleKey.D6, 6);
        public static readonly UserOption Seven = new UserOption("7", ConsoleKey.D7, 7);
        public static readonly UserOption Eight = new UserOption("8", ConsoleKey.D8, 8);
        public static readonly UserOption Nine = new UserOption("9", ConsoleKey.D9, 9);

        public static UserOption[] All => new[] {Zero, One, Two, Three, Four, Five, Six, Seven, Eight, Nine};
    }
}
