namespace WizardsCastle.Logic.Data
{
    internal class Armor
    {
        public Armor(string name, int protection, int durability)
        {
            Name = name;
            Protection = protection;
            Durability = durability;
        }

        public string Name { get; }
        public int Protection { get; }
        public int Durability { get; }

        public override string ToString()
        {
            return $"{Name} [Protection: {Protection} Durability: {Durability}]";
        }
    }
}