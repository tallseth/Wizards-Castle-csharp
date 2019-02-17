﻿using WizardsCastle.Logic.Combat;
using WizardsCastle.Logic.Data;
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
            return null;
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
    }
}