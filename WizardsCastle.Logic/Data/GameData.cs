using System;

namespace WizardsCastle.Logic.Data
{
    internal class GameData
    {
        internal int TurnCounter { get; set; }
        internal Map Map { get; set; }
        internal Player Player { get; set; }
        internal Location CurrentLocation { get; set; }

        internal bool RunestaffDiscovered { get; set; }

        public override string ToString()
        {
            return $"Turn: {TurnCounter} Location: {CurrentLocation}{Environment.NewLine}{Player}";
        }
    }
}