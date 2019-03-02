using System.Reflection;
using NUnit.Framework;
using WizardsCastle.Logic.Data;

namespace WizardsCastle.Logic.Tests
{
    [TestFixture]
    public class GameToolTests
    {
        [Test]
        public void AllMembersNotNull()
        {
            var gameTools = GameTools.Create(GameConfig.Standard);
            var properties = gameTools.GetType().GetProperties(BindingFlags.Instance | BindingFlags.NonPublic);
            foreach (var prop in properties)
            {
                Assert.That(prop.GetMethod.Invoke(gameTools, null), Is.Not.Null, prop.Name);
            }
        }
    }
}