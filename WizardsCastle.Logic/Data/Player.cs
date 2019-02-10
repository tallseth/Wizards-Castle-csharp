namespace WizardsCastle.Logic.Data
{
    internal class Player
    {
        public Player(Race race)
        {
            var raceNum = (int) race;
            Strength = raceNum * 2 + 2;
            Dexterity = 14 - raceNum * 2;
            UnallocatedStats = race == Race.Hobbit ? 4 : 8;
            
            Intelligence = 8;
            GoldPieces = 60;
            Race = race;
        }

        internal Race Race { get; set; }

        internal int Strength { get; set; }
        internal int Dexterity { get; set; }
        internal int Intelligence { get; set; }
        internal int UnallocatedStats { get; set; }

        internal int GoldPieces { get; set; }

        internal bool IsBlind { get; set; }
        internal bool HasOrbOfZot { get; set; }

        public override string ToString()
        {
            return $"ST: {Strength} DE: {Dexterity} IQ: {Intelligence} GP: {GoldPieces}";
        }
    }
}