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
    }
}