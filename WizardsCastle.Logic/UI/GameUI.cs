﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace WizardsCastle.Logic.UI
{
    internal interface IGameUI
    {
        void DisplayMessage(string toDisplay);
        UserOption PromptUserChoice(IEnumerable<UserOption> choices);
        void PromptUserAcknowledgement();
        void ClearActionLog();
    }

    internal class GameUI : IGameUI
    {
        public void DisplayMessage(string toDisplay)
        {
            Console.WriteLine(toDisplay);
        }

        public UserOption PromptUserChoice(IEnumerable<UserOption> choices)
        {
            var dictionary = choices.ToDictionary(c => c.Key);

            while (true)
            {
                var displayString = string.Join(", ", dictionary.Values.Select(option => option.Name));
                WriteStringWithHotKeysHighlighted(displayString);

                var result = Console.ReadKey(true);

                if (dictionary.TryGetValue(result.Key, out var selection))
                    return selection;

                Console.WriteLine("Invalid choice: " + result.KeyChar);
            }
        }

        private void WriteStringWithHotKeysHighlighted(string displayString)
        {
            Console.WriteLine(displayString);
        }

        public void PromptUserAcknowledgement()
        {
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey(true);
        }

        public void ClearActionLog()
        {
            Console.Clear();
        }
    }
}