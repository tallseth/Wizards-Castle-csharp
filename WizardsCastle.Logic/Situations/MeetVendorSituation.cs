using WizardsCastle.Logic.Data;

namespace WizardsCastle.Logic.Situations
{
    internal class MeetVendorSituation : ISituation
    {
        public ISituation PlayThrough(GameData data, GameTools tools)
        {
            //todo: more choices about what action you take here.  Right now I've only implemented "buy stuff", so no need to actually show options.
            tools.UI.DisplayMessage("You have met a Vendor");
            tools.UI.PromptUserAcknowledgement();
            return tools.SituationBuilder.Purchase(tools.InventoryProvider.GetVendorInventory(), tools.SituationBuilder.LeaveRoom());
        }
    }
}