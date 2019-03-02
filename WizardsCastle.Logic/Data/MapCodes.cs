namespace WizardsCastle.Logic.Data
{
    public static class MapCodes
    {
        public const string Entrance = "E";
        public const string EmptyRoom = ".";
        public const string StairsUp = "U";
        public const string StairsDown = "D";
        public const string UnexploredPrefix = "?";
        public const string Gold = "G";
        public const string Vendor = "V";
        public const string MonsterPrefix = "M";
        public const string Warp = "W";
        public const string Sinkhole = "S";
        public const string WarpOfZot = "WZ";

        public static string Unexplored(string roomCode)
        {
            return $"{UnexploredPrefix}{roomCode}";
        }
    }
}