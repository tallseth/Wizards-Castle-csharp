using System;

namespace WizardsCastle.Logic.Data
{
    internal enum Monster
    {
        Kobold = 1,
        Orc = 2,
        Wolf = 3,
        Goblin = 4,
        Ogre = 5,
        Troll = 6,
        Bear = 7,
        Minotaur = 8,
        Gargoyle = 9,
        Chimera = 10,
        Balrog = 11,
        Dragon = 12
    }

    internal static class MonsterHelpers
    {
        public static string GetLocationCode(this Monster monster)
        {
            var num = (int) monster;
            return "M" + num;
        }

        public static Monster ParseLocationCode(string code)
        {
            if(!code.StartsWith("M"))
                throw new ArgumentException($"Invalid string to parse as monster: {code}");

            return (Monster) Convert.ToInt32(code.Substring(1));
        }
    }
}