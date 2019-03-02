using Moq;
using NUnit.Framework;
using WizardsCastle.Logic.Data;
using WizardsCastle.Logic.Situations;
using WizardsCastle.Logic.Tests.Helpers;

namespace WizardsCastle.Logic.Tests.Situations
{
    [TestFixture]
    public class EnterCombatSituationTests
    {
        private ISituation _situation;
        private MockGameTools _tools;
        private GameData _data;

        [SetUp]
        public void Setup()
        {
            _situation = new SituationBuilder().EnterCombat();

            _tools = new MockGameTools();
            _data = Any.GameData();
        }

        [Test]
        public void GoesToPlayerAttackIfPlayerGoesFirst()
        {
            var expected = Mock.Of<ISituation>();
            _tools.CombatServiceMock.Setup(cs => cs.PlayerGoesFirst(_data.Player)).Returns(true);
            _tools.SituationBuilderMock.Setup(sb => sb.CombatOptions()).Returns(expected);

            var actual = _situation.PlayThrough(_data, _tools);

            Assert.That(actual, Is.SameAs(expected));
        }

        [Test]
        public void GoesToEnemyAttackIfPlayerDoesNotGoFirst()
        {
            var expected = Mock.Of<ISituation>();
            _tools.CombatServiceMock.Setup(cs => cs.PlayerGoesFirst(_data.Player)).Returns(false);
            _tools.SituationBuilderMock.Setup(sb => sb.EnemyAttack(false)).Returns(expected);

            var actual = _situation.PlayThrough(_data, _tools);

            Assert.That(actual, Is.SameAs(expected));
        }
    }
}