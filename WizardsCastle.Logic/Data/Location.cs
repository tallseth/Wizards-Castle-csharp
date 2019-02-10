namespace WizardsCastle.Logic.Data
{
    public sealed class Location
    {
        public byte X { get; }
        public byte Y { get; }
        public byte Floor { get; }

        public Location(byte x, byte y, byte floor)
        {
            X = x;
            Y = y;
            Floor = floor;
        }

        private bool Equals(Location other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return X == other.X && Y == other.Y && Floor == other.Floor;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Location);
        }

        public override int GetHashCode()
        {
            var hashCode = X.GetHashCode();
            hashCode = (hashCode * 397) ^ Y.GetHashCode();
            hashCode = (hashCode * 397) ^ Floor.GetHashCode();
            return hashCode;
        }

        public override string ToString()
        {
            return $"{X}, {Y}, {Floor}";
        }
    }
}