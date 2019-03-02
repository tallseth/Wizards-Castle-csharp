using WizardsCastle.Logic.Data;
using WizardsCastle.Logic.Purchases;
using WizardsCastle.Logic.UI;

namespace WizardsCastle.Logic.Situations
{
    internal interface ISituationBuilder
    {
        ISituation Start();
        ISituation CreatePlayer();
        ISituation EnterRoom(Location location);
        ISituation Navigate(UserOption[] entrance);
        ISituation GameOver(string exitMessage);
        ISituation ShowMap();
        ISituation LeaveRoom();
        ISituation EnterCombat();
        ISituation CombatOptions();
        ISituation EnemyAttack(bool playerRetreating);
        ISituation CombatVictory();
        ISituation PlayerAttack();
        ISituation AllocateStats();
        ISituation WarpRoom(bool warpOfZot);
        ISituation Sinkhole();
        ISituation Teleport();
        ISituation Purchase(IPurchaseChoice[] choices, ISituation nextSituation);
        ISituation MeetVendor();
        ISituation ShineLamp();
        ISituation CollectGold();
        ISituation OpenChest();
        ISituation YesOrNo(string message, ISituation yesSituation);
        ISituation DrinkFromPool();
    }

    internal class SituationBuilder : ISituationBuilder
    {
        public ISituation Start()
        {
            return new StartSituation();
        }

        public ISituation CreatePlayer()
        {
            return new PlayerCreationSituation();
        }

        public ISituation EnterRoom(Location location)
        {
            return new EnterRoomSituation(location);
        }

        public ISituation Navigate(UserOption[] movementOptions)
        {
            return new NavigationSituation(movementOptions);
        }

        public ISituation GameOver(string exitMessage)
        {
            return new GameOverSituation(exitMessage);
        }

        public ISituation ShowMap()
        {
            return new ShowMapSituation();
        }

        public ISituation LeaveRoom()
        {
            return new LeaveRoomSituation();
        }

        public ISituation EnterCombat()
        {
            return new EnterCombatSituation();
        }

        public ISituation CombatOptions()
        {
            return new CombatOptionsSituation();
        }

        public ISituation EnemyAttack(bool playerRetreating)
        {
            return new EnemyAttackSituation(playerRetreating);
        }

        public ISituation CombatVictory()
        {
            return new CombatVictorySituation();
        }

        public ISituation PlayerAttack()
        {
            return new PlayerAttackSituation();
        }

        public ISituation AllocateStats()
        {
            return new AllocatePlayerStatsSituation();
        }

        public ISituation WarpRoom(bool warpOfZot)
        {
            return new WarpRoomSituation(warpOfZot);
        }

        public ISituation Sinkhole()
        {
            return new SinkholeSituation();
        }

        public ISituation Teleport()
        {
            return new TeleportSituation();
        }

        public ISituation Purchase(IPurchaseChoice[] choices, ISituation nextSituation)
        {
            return new PurchaseSituation(choices, nextSituation);
        }

        public ISituation MeetVendor()
        {
            return new MeetVendorSituation();
        }

        public ISituation ShineLamp()
        {
            return new ShineLampSituation();
        }

        public ISituation CollectGold()
        {
            return new CollectGoldSituation();
        }

        public ISituation OpenChest()
        {
            return new OpenChestSituation();
        }

        public ISituation YesOrNo(string message, ISituation yesSituation)
        {
            return new YesOrNoSituation(message, yesSituation);
        }

        public ISituation DrinkFromPool()
        {
            return new DrinkFromPoolSituation();
        }

        private class DrinkFromPoolSituation : ISituation
        {
            public ISituation PlayThrough(GameData data, GameTools tools)
            {
                tools.UI.ClearActionLog();
                tools.UI.DisplayMessage(Messages.DrinkFromPool);

                var message = tools.Pool.DrinkFrom(data.Player);

                if (data.Player.IsDead)
                    return tools.SituationBuilder.GameOver(Messages.DieFromPool);

                tools.UI.DisplayMessage(message);
                return tools.SituationBuilder.LeaveRoom();
            }
        }


        private class CollectGoldSituation : ISituation
        {
            public ISituation PlayThrough(GameData data, GameTools tools)
            {
                var foundGold = tools.Randomizer.RollDie(10);
                data.Player.GoldPieces += foundGold;

                tools.UI.ClearActionLog();
                tools.UI.DisplayMessage($"You found {foundGold} Gold.");

                data.Map.SetLocationInfo(data.CurrentLocation, MapCodes.EmptyRoom);
                return tools.SituationBuilder.LeaveRoom();
            }
        }

        private class OpenChestSituation : ISituation
        {
            public ISituation PlayThrough(GameData data, GameTools tools)
            {
                tools.UI.ClearActionLog();

                switch (tools.Randomizer.RollDie(4))
                {
                    case 1:
                        tools.UI.DisplayMessage(Messages.PoisonChestOpens);
                        tools.UI.PromptUserAcknowledgement();
                        tools.UI.DisplayMessage(Messages.PoisonChestAwaken);
                        
                        data.TurnCounter += 20;
                        break;
                    case 2:
                        var result = tools.CombatService.ChestExplodes(data.Player);
                        if (result.DefenderDied)
                            return tools.SituationBuilder.GameOver(Messages.KilledByExplodingChest);
                        tools.UI.DisplayMessage($"The chest explodes as you open it, dealing {result.DamageToDefender} damage.");
                        break;
                    default:
                        var gold = tools.Randomizer.RollDice(5, 200);
                        data.Player.GoldPieces += gold;
                        tools.UI.DisplayMessage($"You open the chest and find {gold} gold pieces.");
                        break;
                }

                data.Map.SetLocationInfo(data.CurrentLocation, MapCodes.EmptyRoom);
                return tools.SituationBuilder.LeaveRoom();
            }
        }

        private class YesOrNoSituation : ISituation
        {
            private readonly string _message;
            private readonly ISituation _yesSituation;

            public YesOrNoSituation(string message, ISituation yesSituation)
            {
                _message = message;
                _yesSituation = yesSituation;
            }

            public ISituation PlayThrough(GameData data, GameTools tools)
            {
                tools.UI.ClearActionLog();
                tools.UI.DisplayMessage(_message);
                if(tools.UI.PromptUserChoice(YesNoOptions.All, true).GetData<bool>())
                    return _yesSituation;

                return tools.SituationBuilder.LeaveRoom();
            }
        }
    }
}