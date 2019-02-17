using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework.Constraints;
using WizardsCastle.Logic.Combat;
using WizardsCastle.Logic.Data;
using WizardsCastle.Logic.UI;

namespace WizardsCastle.Logic.Tests
{
    internal static class Any
    {
        private static readonly Random _random = new Random();

        public static int Number()
        {
            return _random.Next(0, 1000);
        }

        public static string String()
        {
            return Guid.NewGuid().ToString();
        }

        public static T Of<T>(IEnumerable<T> choices)
        {
            var arr = choices.ToArray();
            return arr[_random.Next(arr.Length)];
        }

        public static T Of<T>(params T[] choices)
        {
            return Of((IEnumerable<T>) choices);
        }

        public static T EnumValue<T>()
        {
            return Of(Enum.GetValues(typeof(T)).Cast<T>());
        }

        public static GameData GameData()
        {
            return new GameData
            {
                Player = Player(),
                TurnCounter = Number(),
                CurrentLocation = Location(),
                Map = Map()
            };
        }

        public static Map Map()
        {
            return new Map(new GameConfig{ Floors = byte.MaxValue, FloorWidth = byte.MaxValue, FloorHeight = byte.MaxValue});
        }

        public static Player Player()
        {
            return new Player(EnumValue<Race>())
            {
                GoldPieces = Number(), 
                Intelligence = Number(),
                Strength = Number(),
                Dexterity = Number(),
                UnallocatedStats = Number(),
                Weapon = Weapon(),
                Armor = Armor()
            };
        }

        private static Armor Armor()
        {
            return new Armor(String(), Number(), Number());
        }

        private static Weapon Weapon()
        {
            return new Weapon(String(),Number());
        }

        public static UserOption[] UserOptions()
        {
            return new[]
            {
                UserOptionWithValue(Number()),
                UserOptionWithValue(Number()),
                UserOptionWithValue(Number())
            };
        }

        public static Move InsideMove()
        {
            return Of(Enum.GetValues(typeof(Move)).Cast<Move>().Where(m => m != Move.Exit && m != Move.ShowMap));
        }

        public static UserOption UserOptionWithValue(object obj)
        {
            return new UserOption(String(), EnumValue<ConsoleKey>(), obj);
        }

        public static Location Location()
        {
            return new Location(Byte(), Byte(), Byte());
        }

        public static byte Byte()
        {
            return (byte) _random.Next(byte.MaxValue);
        }

        public static Enemy Monster()
        {
            return Enemy.CreateMonster(EnumValue<Monster>());
        }
    }
}