namespace WizardsCastle.Logic.Data
{
    public static class MapCodes
    {
        public const char Entrance = 'E';
        public const char EmptyRoom = '.';
        public const char StairsUp = 'U';
        public const char StairsDown = 'D';
        public const char UnexploredPrefix = '?';
        public const char Gold = 'G';
        public const char Vendor = 'V';
        public const char MonsterPrefix = 'M';
        public const char Warp = 'W';
        public const char Sinkhole = 'S';
        public const string WarpOfZot = "WZ";

        public static string Unexplored(char roomCode)
        {
            return Unexplored(roomCode.ToString());
        }

        public static string Unexplored(string roomCode)
        {
            return $"{UnexploredPrefix}{roomCode}";
        }
    }
}