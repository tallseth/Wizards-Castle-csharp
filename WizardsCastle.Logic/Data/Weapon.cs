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
    }
}