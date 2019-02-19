using System.Collections.Generic;
using WizardsCastle.Logic.Data;
using WizardsCastle.Logic.UI;

namespace WizardsCastle.Logic.Situations
{
    internal class AllocatePlayerStatsSituation : ISituation
    {
        public ISituation PlayThrough(GameData data, GameTools tools)
        {
            tools.UI.ClearActionLog();
            tools.UI.DisplayMessage($"You have {data.Player.UnallocatedStats} points to allocate.");
            tools.UI.DisplayMessage(data.Player.ToString());
            
            var choice = tools.UI.PromptUserChoice(StatsOptions.All, true).GetData<char>();

            switch (choice)
            {
                case 'S':
                    data.Player.Strength++;
                    break;
                case 'D':
                    data.Player.Dexterity++;
                    break;
                case 'I':
                    data.Player.Intelligence++;
                    break;
            }

            data.Player.UnallocatedStats--;

            if (data.Player.UnallocatedStats > 0)
                return tools.SituationBuilder.AllocateStats();

            return tools.SituationBuilder.EnterRoom(data.CurrentLocation);
        }
    }
}