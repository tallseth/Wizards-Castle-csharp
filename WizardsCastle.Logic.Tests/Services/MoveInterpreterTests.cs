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
            var config = new GameConfig { Floors = 3, FloorWidth = 3, FloorHeight = 3};
            _interpreter = new MoveInterpreter(config);
        }

        [Test]
        public void StandardMoves()
        {
            var middle = new Location(1, 1, 1);
            TestMove(middle, Move.Left, new Location(0, 1, 1));
            TestMove(middle, Move.Right, new Location(2, 1, 1));
            TestMove(middle, Move.Up, new Location(1, 0, 1));
            TestMove(middle, Move.Down, new Location(1, 2, 1));
            TestMove(middle, Move.Downstairs, new Location(1, 1, 0));
            TestMove(middle, Move.Upstairs, new Location(1, 1, 2));
        }

        [Test]
        public void WrapAroundMoves()
        {
            var topLeftCorner = new Location(0, 0, 0);
            TestMove(topLeftCorner, Move.Left, new Location(2, 0, 0));
            TestMove(topLeftCorner, Move.Downstairs, new Location(0, 0, 2));
            TestMove(topLeftCorner, Move.Up, new Location(0, 2, 0));

            var bottomRightCorner = new Location(2, 2, 2);
            TestMove(bottomRightCorner, Move.Right, new Location(0, 2, 2));
            TestMove(bottomRightCorner, Move.Down, new Location(2, 0, 2));
            TestMove(bottomRightCorner, Move.Upstairs, new Location(2, 2, 0));
        }

        private void TestMove(Location origin, Move move, Location expectedLocation)
        {
            var result = _interpreter.GetTargetLocation(origin, move);
            Assert.That(result, Is.EqualTo(expectedLocation));
        }
    }
}