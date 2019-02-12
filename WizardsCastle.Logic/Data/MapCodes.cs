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

        public static string Unexplored(char roomCode)
        {
            return $"{UnexploredPrefix}{roomCode}";
        }
    }
}