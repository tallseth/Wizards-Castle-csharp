﻿using NUnit.Framework;
using WizardsCastle.Logic.Data;
using WizardsCastle.Logic.Situations;
using WizardsCastle.Logic.Tests.Helpers;

namespace WizardsCastle.Logic.Tests.Situations
{
    [TestFixture]
    public class CombatVictorySituationTests
    {
        private ISituation _situation;
        private MockGameTools _tools;
        private GameData _data;

        [SetUp]
        public void Setup()
        {
            _situation = new SituationBuilder().CombatVictory();

            _tools = new MockGameTools();
            _data = Any.GameData();
        }

        [Test]
        public void VictorGathersRewardAndLeavesTheRoomWhenDefeatingMonster()
        {
            var enemy = _tools.SetupEnemyAtCurrentLocation(_data);
            enemy.IsMonster = true;
            var next = _tools.SetupNextSituation(sb => sb.LeaveRoom());
            var lootMessage = Any.String();
            _tools.LootCollectorMock.Setup(lc => lc.CollectMonsterLoot(_data)).Returns(lootMessage);

            var actual = _situation.PlayThrough(_data, _tools);

            Assert.That(actual, Is.SameAs(next));
            _tools.UIMock.Verify(ui=>ui.DisplayMessage(lootMessage));
        }

        [Test]
        public void VictorGathersRewardAndLeavesTheRoomWhenDefeatingVendor()
        {
            var angryVendor = _tools.SetupEnemyAtCurrentLocation(_data);
            angryVendor.IsMonster = false;
            var next = _tools.SetupNextSituation(sb => sb.LeaveRoom());
            var lootMessage = Any.String();
            _tools.LootCollectorMock.Setup(lc => lc.CollectVendorLoot(_data)).Returns(lootMessage);

            var actual = _situation.PlayThrough(_data, _tools);

            Assert.That(actual, Is.SameAs(next));
            _tools.UIMock.Verify(ui=>ui.DisplayMessage(lootMessage));
        }
    }
}