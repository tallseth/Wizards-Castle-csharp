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
        public int Durability { get; set; }

        public override string ToString()
        {
            return $"{Name} [Protection: {Protection} Durability: {Durability}]";
        }

        public static readonly Armor Leather = new Armor("Leather", 1, 7);
        public static readonly Armor Chainmail = new Armor("Chainmail", 2, 14);
        public static readonly Armor Plate = new Armor("Plate", 3, 21);
    }
}