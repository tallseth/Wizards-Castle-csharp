using WizardsCastle.Logic.Data;

namespace WizardsCastle.Logic.Purchases
{
    internal interface IPurchaseChoice
    {
        string Name { get; }
        int Cost { get; }
        void Apply(Player player);
    }
}