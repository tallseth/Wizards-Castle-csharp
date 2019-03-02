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

        internal Weapon Weapon { get; set; }
        internal Armor Armor { get; set; }

        internal int MonstersDefeated { get; set; }

        internal bool IsBlind { get; set; }
        internal Curses Curses { get; set; }

        internal bool HasOrbOfZot { get; set; }
        internal bool HasRuneStaff { get; set; }
        internal bool HasLamp { get; set; }

        public bool IsDead => Strength < 1 || Dexterity < 1 || Intelligence < 1;

        public override string ToString()
        {
            return $"ST: {Strength} DE: {Dexterity} IQ: {Intelligence} GP: {GoldPieces} {Weapon?.ToString() ?? "[No Weapon]"} {Armor?.ToString() ?? "[No Armor]"}";
        }
    }
}