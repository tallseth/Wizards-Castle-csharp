using WizardsCastle.Logic.Data;

namespace WizardsCastle.Logic.Situations
{
    internal class EnterCombatSituation : ISituation
    {
        public ISituation PlayThrough(GameData data, GameTools tools)
        {
            if(tools.CombatService.PlayerGoesFirst(data.Player))
                return tools.SituationBuilder.CombatOptions();

            return tools.SituationBuilder.EnemyAttack(false);
        }
    }
}