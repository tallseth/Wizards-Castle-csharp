namespace WizardsCastle.Logic.Data
{
    internal class GameData
    {
        internal int TurnCounter { get; set; }
        internal Map Map { get; set; }
        internal Player Player { get; set; }
        internal Location CurrentLocation { get; set; }
    }
}