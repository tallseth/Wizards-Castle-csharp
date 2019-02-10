using NUnit.Framework;
using WizardsCastle.Logic.Situations;

namespace WizardsCastle.Logic.Tests.Situations
{
    [TestFixture]
    public class SituationBuilderTests
    {
        private SituationBuilder _builder;

        [SetUp]
        public void Setup()
        {
            _builder = new SituationBuilder();
        }

        [Test]
        public void StartSituationsIsCorrectType()
        {
            var situation = _builder.Start();

            Assert.That(situation, Is.TypeOf<StartSituation>());
        }
    }
}