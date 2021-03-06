﻿using WizardsCastle.Logic.Data;

namespace WizardsCastle.Logic.Combat
{
    internal class Enemy
    {
        private Enemy(string name, int combatNumber, bool stoneSkin, bool isMonster)
        {
            Name = name;
            StoneSkin = stoneSkin;
            IsMonster = isMonster;
            HitPoints = combatNumber+2;
            Damage = (combatNumber / 2) + 1;
        }

        public static Enemy CreateMonster(Monster type)
        {
            var stoneSkin = (type == Monster.Gargoyle || type == Monster.Dragon);
            return new Enemy(type.ToString(), (int) type, stoneSkin, true);
        }

        public static Enemy CreateVendorCombatant()
        {
            return new Enemy("Vendor", 13, false, false);
        }

        public int HitPoints { get; set; }
        public int Damage { get; set; }
        public string Name { get; set; }
        public bool StoneSkin { get; set; }
        public bool IsMonster { get; set; }

        public override string ToString()
        {
            return $"{Name} HP: {HitPoints}";
        }
    }
}
