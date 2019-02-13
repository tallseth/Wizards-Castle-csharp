using NUnit.Framework;
using WizardsCastle.Logic.Data;
using WizardsCastle.Logic.Services;

namespace WizardsCastle.Logic.Tests.Services
{
    [TestFixture]
    public class MoveInterpreterTests
    {
        private MoveInterpreter _interpreter;

        [SetUp]
        public void Setup()
        {
            var config = new GameConfig { Floors = 5, FloorWidth = 5, FloorHeight = 5};
            _interpreter = new MoveInterpreter(config);
        }

        [Test]
        public void StandardMoves()
        {
            var middle = new Location(1, 2, 3);
            TestMove(middle, Move.Left, new Location(0, 2, 3));
            TestMove(middle, Move.Right, new Location(2, 2, 3));
            TestMove(middle, Move.Up, new Location(1, 1, 3));
            TestMove(middle, Move.Down, new Location(1, 3, 3));
            TestMove(middle, Move.Downstairs, new Location(1, 2, 2));
            TestMove(middle, Move.Upstairs, new Location(1, 2, 4));
        }

        [Test]
        public void WrapAroundMoves()
        {
            var topLeftCorner = new Location(0, 0, 0);
            TestMove(topLeftCorner, Move.Left, new Location(4, 0, 0));
            TestMove(topLeftCorner, Move.Downstairs, new Location(0, 0, 4));
            TestMove(topLeftCorner, Move.Up, new Location(0, 4, 0));

            var bottomRightCorner = new Location(4, 4, 4);
            TestMove(bottomRightCorner, Move.Right, new Location(0, 4, 4));
            TestMove(bottomRightCorner, Move.Down, new Location(4, 0, 4));
            TestMove(bottomRightCorner, Move.Upstairs, new Location(4, 4, 0));
        }

        private void TestMove(Location origin, Move move, Location expectedLocation)
        {
            var result = _interpreter.GetTargetLocation(origin, move);
            Assert.That(result, Is.EqualTo(expectedLocation));
        }
    }
}