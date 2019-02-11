using WizardsCastle.Logic.Data;

namespace WizardsCastle.Logic.Combat
{
    internal class Enemy
    {
        private Enemy(string name, int combatNumber)
        {
            Name = name;
            HitPoints = combatNumber+2;
            Damage = (combatNumber / 2) + 1;
        }

        public static Enemy CreateMonster(Monster type)
        {
            return new Enemy(type.ToString(), (int) type);
        }

        public static Enemy CreateVendorCombatant()
        {
            return new Enemy("Vendor", 13);
        }

        public int HitPoints { get; set; }
        public int Damage { get; }
        public string Name { get; }
    }
}
