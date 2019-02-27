using System;
using WizardsCastle.Logic.Data;
using WizardsCastle.Logic.Services;

namespace WizardsCastle.Logic.Purchases
{
    internal interface IInventoryProvider
    {
        IPurchaseChoice[] GetPlayerCreationInventory();
        IPurchaseChoice[] GetVendorInventory();
    }

    internal class InventoryProvider : IInventoryProvider
    {
        private readonly IRandomizer _randomizer;

        public InventoryProvider(IRandomizer randomizer)
        {
            _randomizer = randomizer;
        }

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

        public IPurchaseChoice[] GetVendorInventory()
        {
            return new IPurchaseChoice[]
            {
                new WeaponPurchase(Weapon.Dagger, 1250),
                new WeaponPurchase(Weapon.Mace, 1500),
                new WeaponPurchase(Weapon.Sword, 2000),
                new ArmorPurchase(Armor.Leather, 1250),
                new ArmorPurchase(Armor.Chainmail, 1500),
                new ArmorPurchase(Armor.Plate, 2000),
                new PotionPurchase("Strength", IncreaseStrength), 
                new PotionPurchase("Dexterity", IncreaseDexterity), 
                new PotionPurchase("Intelligence", IncreaseIntelligence), 
            };
        }

        private void IncreaseStrength(Player player)
        {
            player.Strength = Math.Min(_randomizer.RollDie(6) + player.Strength, 18);
        }

        private void IncreaseDexterity(Player player)
        {
            player.Dexterity = Math.Min(_randomizer.RollDie(6) + player.Dexterity, 18);
        }

        private void IncreaseIntelligence(Player player)
        {
            player.Intelligence = Math.Min(_randomizer.RollDie(6) + player.Intelligence, 18);
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

        private class PotionPurchase : IPurchaseChoice
        {
            private readonly Action<Player> _effect;

            public PotionPurchase(string attribute, Action<Player> effect)
            {
                _effect = effect;
                Name = "Potion of " + attribute;
                Cost = 1000;
            }

            public string Name { get; }
            public int Cost { get; }
            public void Apply(Player player)
            {
                _effect(player);
            }
        }
    }
}