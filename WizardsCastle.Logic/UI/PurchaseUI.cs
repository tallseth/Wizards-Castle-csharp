using System;
using System.Collections.Generic;
using System.Linq;
using WizardsCastle.Logic.Purchases;

namespace WizardsCastle.Logic.UI
{
    internal interface IPurchaseUI
    {
        bool OfferPurchaseOptions(IPurchaseChoice[] choices, int availableGold, out IPurchaseChoice selection);
        void NotifyInsufficientFunds();
    }

    internal class PurchaseUI : IPurchaseUI
    {
        private readonly IGameUI _mainUI;

        public PurchaseUI(IGameUI mainUI)
        {
            _mainUI = mainUI;
        }

        public bool OfferPurchaseOptions(IPurchaseChoice[] choices, int availableGold, out IPurchaseChoice selection)
        {
            _mainUI.ClearActionLog();

            _mainUI.DisplayMessage(GetHeader(availableGold));
            for (int i = 0; i < choices.Length; i++)
            {
                _mainUI.DisplayMessage(GetChoiceDisplay(choices[i], i));
            }
            _mainUI.DisplayMessage(GetFooter());
            var chosen = _mainUI.PromptUserChoice(GetNumberedChoices(choices.Length), false).GetData<int>();

            if (chosen == -1)
            {
                selection = null;
                return false;
            }

            selection = choices[chosen];
            return true;
        }

        private IEnumerable<UserOption> GetNumberedChoices(int choicesLength)
        {
            foreach (var o in NumberOptions.All.Take(choicesLength))
            {
                yield return o;
            }

            yield return new UserOption("(L)eave", ConsoleKey.L, -1);
        }

        private string GetFooter()
        {
            return "\n\rChoose an item, or (L)eave.";
        }

        private string GetChoiceDisplay(IPurchaseChoice purchaseChoice, int i)
        {
            return $"| {i} | {purchaseChoice.Name.PadRight(24)}| {purchaseChoice.Cost.ToString().PadRight(6)}|";
        }

        private string GetSeparator()
        {
            return "";
        }

        private string GetHeader(int gold)
        {
            return $@"
You have {gold} gold pieces.

| # | Item                    | Price |
|:-:|:-----------------------:|:-----:|";
        }

        public void NotifyInsufficientFunds()
        {
            _mainUI.DisplayMessage("You do not have enough gold for that choice.  Please make another selection.");
            _mainUI.PromptUserAcknowledgement();
        }
    }
}