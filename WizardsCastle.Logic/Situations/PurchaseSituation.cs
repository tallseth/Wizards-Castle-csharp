using WizardsCastle.Logic.Data;
using WizardsCastle.Logic.Purchases;

namespace WizardsCastle.Logic.Situations
{
    internal class PurchaseSituation : ISituation
    {
        private readonly IPurchaseChoice[] _choices;
        private readonly ISituation _nextSituation;

        public PurchaseSituation(IPurchaseChoice[] choices, ISituation nextSituation)
        {
            _choices = choices;
            _nextSituation = nextSituation;
        }

        public ISituation PlayThrough(GameData data, GameTools tools)
        {
            tools.UI.ClearActionLog();
            tools.UI.DisplayMessage(data.Player.ToString());

            if (!tools.PurchaseUI.OfferPurchaseOptions(_choices, out var result))
                return _nextSituation;

            if (result.Cost > data.Player.GoldPieces)
            {
                tools.PurchaseUI.NotifyInsufficientFunds();
            }
            else
            {
                result.Apply(data.Player);
                data.Player.GoldPieces -= result.Cost;
            }

            return this;
        }
    }
}