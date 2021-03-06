﻿using System;
using System.Linq.Expressions;
using Moq;
using WizardsCastle.Logic.Combat;
using WizardsCastle.Logic.Data;
using WizardsCastle.Logic.Purchases;
using WizardsCastle.Logic.Services;
using WizardsCastle.Logic.Situations;
using WizardsCastle.Logic.UI;

namespace WizardsCastle.Logic.Tests.Helpers
{
    internal class MockGameTools : GameTools
    {
        public Mock<IGameUI> UIMock { get; }
        public Mock<IGameDataBuilder> GameDataBuilderMock { get; }
        public Mock<ISituationBuilder> SituationBuilderMock { get; }
        public Mock<IMoveInterpreter> MoveInterpreterMock { get; }
        public Mock<IRandomizer> RandomizerMock { get; }
        public Mock<ICombatService> CombatServiceMock { get; }
        public Mock<IEnemyProvider> EnemyProviderMock { get; }
        public Mock<ICurseEvaluator> CurseEvaluatorMock { get; }
        public Mock<ICombatDice> CombatDiceMock { get; }
        public Mock<IRoomEnumerator> RoomEnumeratorMock { get; }
        public Mock<ILootCollector> LootCollectorMock { get; }
        public Mock<ITeleportUI> TeleportUIMock { get; }
        public Mock<IPurchaseUI> PurchaseUIMock { get; }
        public Mock<IInventoryProvider> InventoryProviderMock { get; }
        public Mock<IPool> PoolMock { get; }


        public MockGameTools()
        {
            UIMock = new Mock<IGameUI>();
            UI = UIMock.Object;

            GameDataBuilderMock = new Mock<IGameDataBuilder>();
            DataBuilder = GameDataBuilderMock.Object;

            SituationBuilderMock = new Mock<ISituationBuilder>();
            SituationBuilder = SituationBuilderMock.Object;

            MoveInterpreterMock = new Mock<IMoveInterpreter>();
            MoveInterpreter = MoveInterpreterMock.Object;

            RandomizerMock = new Mock<IRandomizer>();
            Randomizer = RandomizerMock.Object;

            CombatServiceMock = new Mock<ICombatService>();
            CombatService = CombatServiceMock.Object;

            EnemyProviderMock = new Mock<IEnemyProvider>();
            EnemyProvider = EnemyProviderMock.Object;

            CurseEvaluatorMock = new Mock<ICurseEvaluator>();
            CurseEvaluator = CurseEvaluatorMock.Object;

            CombatDiceMock = new Mock<ICombatDice>();
            CombatDice = CombatDiceMock.Object;

            RoomEnumeratorMock = new Mock<IRoomEnumerator>();
            RoomEnumerator = RoomEnumeratorMock.Object;

            LootCollectorMock = new Mock<ILootCollector>();
            LootCollector = LootCollector = LootCollectorMock.Object;

            TeleportUIMock = new Mock<ITeleportUI>();
            TeleportUI = TeleportUIMock.Object;

            PurchaseUIMock = new Mock<IPurchaseUI>();
            PurchaseUI = PurchaseUIMock.Object;

            InventoryProviderMock = new Mock<IInventoryProvider>();
            InventoryProvider = InventoryProviderMock.Object;

            PoolMock = new Mock<IPool>();
            Pool = PoolMock.Object;
        }

        public ISituation SetupNextSituation(Expression<Func<ISituationBuilder, ISituation>> expression)
        {
            var situation = Mock.Of<ISituation>();
            SituationBuilderMock.Setup(expression).Returns(situation);
            return situation;
        }

        public Enemy SetupEnemyAtCurrentLocation(GameData data)
        {
            var enemy = Any.Monster();
            EnemyProviderMock.Setup(p => p.GetEnemy(data.Map, data.CurrentLocation)).Returns(enemy);
            return enemy;
        }
    }
}