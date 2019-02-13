using WizardsCastle.Logic.Data;

namespace WizardsCastle.Logic.Services
{
    internal interface IMoveInterpreter
    {
        Location GetTargetLocation(Location origin, Move move);
    }

    internal class MoveInterpreter : IMoveInterpreter
    {
        
        private readonly GameConfig _config;

        public MoveInterpreter(GameConfig config)
        {
            _config = config;
        }

        public Location GetTargetLocation(Location origin, Move move)
        {
            var x = origin.X;
            var y = origin.Y;
            var z = origin.Floor;

            switch (move)
            {
                case Move.Up:
                    y = Adjust(y, -1, _config.FloorHeight);
                    break;
                case Move.Down:
                    y = Adjust(y, 1, _config.FloorHeight);
                    break;
                case Move.Left:
                    x = Adjust(x, -1, _config.FloorWidth);
                    break;
                case Move.Right:
                    x = Adjust(x, 1, _config.FloorWidth);
                    break;
                case Move.Upstairs:
                    z = Adjust(z, 1, _config.Floors);
                    break;
                case Move.Downstairs:
                    z = Adjust(z, -1, _config.Floors);
                    break;
            }

            return new Location(x,y,z);
        }

        private byte Adjust(byte toAdjust, int addend, byte modulus)
        {
            if (toAdjust == 0)
                toAdjust = modulus;

            var newByte = (byte) (toAdjust + addend);
            var result = (byte) (newByte % modulus);
            return result;
        }
    }
}