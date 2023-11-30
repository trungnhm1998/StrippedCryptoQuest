using CryptoQuest.Item;
using CryptoQuest.Item.Consumable;
using CryptoQuest.Item.Equipment;
using CryptoQuest.System;
using UnityEngine;

namespace CryptoQuest.Shop
{
    public interface IShopInventoryController
    {
        public bool TryToBuy(EquipmentInfo equipmentItem);
        public bool TryToBuy(ConsumableInfo equipmentItem);
        public bool TryToSell(EquipmentInfo equipmentItem);
        public bool TryToSell(ConsumableInfo equipmentItem);
    }

    public class ShopInventoryController : MonoBehaviour, IShopInventoryController
    {
        private void Awake()
        {
            ServiceProvider.Provide<IShopInventoryController>(this);
        }

        private bool HasEnoughGold(ItemInfo shopItem)
            // => _inventoryController.Inventory.WalletController.Wallet.Gold.Amount >= shopItem.Price;
            => false; // REFACTOR: SHOP

        // TODO: REFACTOR SHOP
        public bool TryToBuy(EquipmentInfo equipmentItem)
        {
            // if (!HasEnoughGold(equipmentItem)) return false;
            // if (!_lootController.Add(equipmentItem)) return false;

            // _inventoryController.Inventory.WalletController.Wallet.Gold.UpdateCurrencyAmount(-equipmentItem.Price);

            return true;
        }

        public bool TryToBuy(ConsumableInfo consumable)
        {
            if (!HasEnoughGold(consumable)) return false;
            // if (!_lootController.Add(consumable)) return false;

            // _inventoryController.Inventory.WalletController.Wallet.Gold.UpdateCurrencyAmount(-consumable.Price);
            // TODO: REFACTOR SHOP

            return true;
        }

        public bool TryToSell(EquipmentInfo equipmentItem)
        {
            // if (!_lootController.Remove(equipmentItem)) return false;

            // _inventoryController.Inventory.WalletController.Wallet.Gold.UpdateCurrencyAmount(equipmentItem.SellPrice);
            // TODO: REFACTOR SHOP

            return true;
        }

        public bool TryToSell(ConsumableInfo consumable)
        {
            // if (!_lootController.Inventory.Remove(consumable)) return false;

            // _inventoryController.Inventory.WalletController.Wallet.Gold.UpdateCurrencyAmount(consumable.SellPrice);
            // TODO: REFACTOR SHOP

            return true;
        }
    }
}