using System;
using WizardsCastle.Logic.Data;
using WizardsCastle.Logic.UI;

namespace WizardsCastle.Logic.Situations
{
    internal class CombatOptionsSituation : ISituation
    {
        public ISituation PlayThrough(GameData data, GameTools tools)
        {
            var enemy = tools.EnemyProvider.GetEnemy(data.Map, data.CurrentLocation);

            data.TurnCounter++;
            tools.UI.DisplayMessage(data.ToString());
            tools.UI.DisplayMessage(enemy.ToString());

            var choice = tools.UI.PromptUserChoice(CombatOptions.All).GetData<char>();

            tools.UI.ClearActionLog();

            switch (choice)
            {
                case 'A':
                    return tools.SituationBuilder.PlayerAttack();
                case 'R':
                    return tools.SituationBuilder.EnemyAttack(true);
                default:
                    throw new InvalidOperationException($"Invalid choice '{choice}'");
            }
        }
    }
}