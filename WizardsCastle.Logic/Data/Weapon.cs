namespace WizardsCastle.Logic.Data
{
    internal class Weapon
    {
        public Weapon(string name, int damage)
        {
            Name = name;
            Damage = damage;
        }

        public string Name { get; }
        public int Damage { get; }

        public override string ToString()
        {
            return $"{Name} [Damage: {Damage}]";
        }


        public static readonly Weapon Dagger = new Weapon("Dagger", 1);
        public static readonly Weapon Mace = new Weapon("Mace", 2);
        public static readonly Weapon Sword = new Weapon("Sword", 3);
    }
}