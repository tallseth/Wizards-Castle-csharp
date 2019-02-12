namespace WizardsCastle.Logic.Data
{
    public class GameConfig
    {
        internal GameConfig()
        {
            
        }

        internal Location Entrance { get; set; }
        internal byte Floors { get; set; }
        internal byte FloorWidth { get; set; }
        internal byte FloorHeight { get; set; }
        internal byte StairsPerFloor { get; set; }

        public static readonly GameConfig Standard = new GameConfig
        {
            Entrance = new Location(3, 0, 0),
            FloorHeight = 8,
            FloorWidth = 8,
            Floors = 8,
            StairsPerFloor = 2
        };

        public static readonly GameConfig Small = new GameConfig
        {
            Entrance = new Location(2, 0, 0),
            FloorHeight = 5,
            FloorWidth = 5,
            Floors = 2,
            StairsPerFloor = 1
        };
    }
}