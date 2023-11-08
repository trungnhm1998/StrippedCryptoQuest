using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.Item;
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
        [SerializeField] private InventoryController _inventoryController;

        private void Awake()
        {
            ServiceProvider.Provide<IShopInventoryController>(this);
        }

        private bool HasEnoughGold(ItemInfo shopItem)
            // => _inventoryController.Inventory.WalletController.Wallet.Gold.Amount >= shopItem.Price;
            => false; // REFACTOR: SHOP

        public bool TryToBuy(EquipmentInfo equipmentItem)
        {
            if (!HasEnoughGold(equipmentItem)) return false;
            if (!_inventoryController.Add(equipmentItem)) return false;

            // _inventoryController.Inventory.WalletController.Wallet.Gold.UpdateCurrencyAmount(-equipmentItem.Price);
            // TODO: REFACTOR SHOP

            return true;
        }

        public bool TryToBuy(ConsumableInfo consumable)
        {
            if (!HasEnoughGold(consumable)) return false;
            if (!_inventoryController.Add(consumable)) return false;

            // _inventoryController.Inventory.WalletController.Wallet.Gold.UpdateCurrencyAmount(-consumable.Price);
            // TODO: REFACTOR SHOP

            return true;
        }

        public bool TryToSell(EquipmentInfo equipmentItem)
        {
            if (!_inventoryController.Remove(equipmentItem)) return false;

            // _inventoryController.Inventory.WalletController.Wallet.Gold.UpdateCurrencyAmount(equipmentItem.SellPrice);
            // TODO: REFACTOR SHOP

            return true;
        }

        public bool TryToSell(ConsumableInfo consumable)
        {
            if (!_inventoryController.Inventory.Remove(consumable)) return false;

            // _inventoryController.Inventory.WalletController.Wallet.Gold.UpdateCurrencyAmount(consumable.SellPrice);
            // TODO: REFACTOR SHOP

            return true;
        }
    }
}