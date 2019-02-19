using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace WizardsCastle.Logic.UI
{
    internal interface IGameUI
    {
        void DisplayMessage(string toDisplay);
        UserOption PromptUserChoice(IEnumerable<UserOption> choices, bool displayChoices);
        void PromptUserAcknowledgement();
        void ClearActionLog();
    }

    internal static class Temp
    {
    }

    internal class GameUI : IGameUI
    {
        public void DisplayMessage(string toDisplay)
        {
            Console.WriteLine(toDisplay);
        }

        public UserOption PromptUserChoice(IEnumerable<UserOption> choices, bool displayChoices)
        {
            var dictionary = choices.ToDictionary(c => c.Key);

            while (true)
            {
                if (displayChoices)
                {
                    Console.WriteLine();
                    var displayString = string.Join(", ", dictionary.Values.Select(option => option.Name));
                    Console.WriteLine(displayString);
                }

                var result = Console.ReadKey(true);

                if (dictionary.TryGetValue(result.Key, out var selection))
                    return selection;

                Console.WriteLine("Invalid choice: " + result.KeyChar);
            }
        }

        public void PromptUserAcknowledgement()
        {
            Console.ReadKey(true);
        }

        public void ClearActionLog()
        {
            Console.Clear();
        }
    }
}