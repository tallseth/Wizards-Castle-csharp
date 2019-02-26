using WizardsCastle.Logic.Data;

namespace WizardsCastle.Logic.Purchases
{
    internal interface IInventoryProvider
    {
        IPurchaseChoice[] GetPlayerCreationInventory();
    }

    internal class InventoryProvider : IInventoryProvider
    {
        public IPurchaseChoice[] GetPlayerCreationInventory()
        {
            return new IPurchaseChoice[]
            {
                new WeaponPurchase(Weapon.Dagger, 10),
                new WeaponPurchase(Weapon.Mace, 20),
                new WeaponPurchase(Weapon.Sword, 30),
                new ArmorPurchase(Armor.Leather, 10),
                new ArmorPurchase(Armor.Chainmail, 20),
                new ArmorPurchase(Armor.Plate, 30),
            };
        }

        private class WeaponPurchase : IPurchaseChoice
        {
            private readonly Weapon _weapon;

            public WeaponPurchase(Weapon weapon, int cost)
            {
                Cost = cost;
                _weapon = weapon;
            }

            public string Name => _weapon.Name;
            public int Cost { get; }
            public void Apply(Player player)
            {
                player.Weapon = _weapon;
            }
        }

        private class ArmorPurchase : IPurchaseChoice
        {
            private readonly Armor _armor;

            public ArmorPurchase(Armor armor, int cost)
            {
                Cost = cost;
                _armor = armor;
            }

            public string Name => _armor.Name;
            public int Cost { get; }
            public void Apply(Player player)
            {
                player.Armor = _armor;
            }
        }
    }
}