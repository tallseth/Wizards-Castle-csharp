using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using WizardsCastle.Logic.Combat;
using WizardsCastle.Logic.Data;
using WizardsCastle.Logic.Situations;
using WizardsCastle.Logic.Tests.Helpers;
using WizardsCastle.Logic.UI;

namespace WizardsCastle.Logic.Tests.Situations
{
    [TestFixture]
    public class CombatOptionsSituationTests
    {
        private ISituation _situation;
        private MockGameTools _tools;
        private GameData _data;
        private Enemy _enemy;

        [SetUp]
        public void Setup()
        {
            _situation = new CombatOptionsSituation();

            _tools = new MockGameTools();
            _data = Any.GameData();

            _enemy = _tools.SetupEnemyAtCurrentLocation(_data);
            _tools.UIMock.Setup(ui => ui.PromptUserChoice(CombatOptions.All)).Returns(CombatOptions.Attack);
        }

        [Test]
        public void IncrementsTurnCounter()
        {
            var originalCounter = _data.TurnCounter;
            
            _situation.PlayThrough(_data, _tools);

            Assert.That(_data.TurnCounter, Is.EqualTo(originalCounter + 1));
        }

        [Test]
        public void DisplaysStatusBeforeOptions()
        {
            _tools.UIMock.Setup(ui => ui.DisplayMessage(It.IsAny<string>()))
                .Callback(() => { _tools.UIMock.Verify(ui => ui.PromptUserChoice(It.IsAny<IEnumerable<UserOption>>()),Times.Never()); });

            _situation.PlayThrough(_data, _tools);

            _tools.UIMock.Verify(ui=>ui.DisplayMessage(_data.ToString()));
            _tools.UIMock.Verify(ui=>ui.DisplayMessage(_enemy.ToString()));
        }

        [Test]
        public void ChoosingAttackGoesToPlayerAttack()
        {
            var next = _tools.SetupNextSituation(sb => sb.PlayerAttack());
            _tools.UIMock.Setup(ui => ui.PromptUserChoice(CombatOptions.All)).Returns(CombatOptions.Attack);

            var actual = _situation.PlayThrough(_data, _tools);

            Assert.That(actual, Is.SameAs(next));
        }

        [Test]
        public void ChoosingRetreatLetsEnemyAttackOneLastTime()
        {
            var next = _tools.SetupNextSituation(sb => sb.EnemyAttack(true));
            _tools.UIMock.Setup(ui => ui.PromptUserChoice(CombatOptions.All)).Returns(CombatOptions.Retreat);

            var actual = _situation.PlayThrough(_data, _tools);

            Assert.That(actual, Is.SameAs(next));
        }
    }
}