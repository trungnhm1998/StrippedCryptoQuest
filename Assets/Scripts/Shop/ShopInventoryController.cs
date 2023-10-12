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
            => _inventoryController.Inventory.WalletController.Wallet.Gold.Amount >= shopItem.Price;

        public bool TryToBuy(EquipmentInfo equipmentItem)
        {
            if (!HasEnoughGold(equipmentItem)) return false;
            if (!_inventoryController.Inventory.Add(equipmentItem)) return false;

            _inventoryController.Inventory.WalletController.Wallet.Gold.UpdateCurrencyAmount(-equipmentItem.Price);

            return true;
        }

        public bool TryToBuy(ConsumableInfo consumable)
        {
            if (!HasEnoughGold(consumable)) return false;
            if (!_inventoryController.Inventory.Add(consumable)) return false;

            _inventoryController.Inventory.WalletController.Wallet.Gold.UpdateCurrencyAmount(-consumable.Price);

            return true;
        }

        public bool TryToSell(EquipmentInfo equipmentItem)
        {
            if (!_inventoryController.Inventory.Remove(equipmentItem)) return false;

            _inventoryController.Inventory.WalletController.Wallet.Gold.UpdateCurrencyAmount(equipmentItem.SellPrice);

            return true;
        }

        public bool TryToSell(ConsumableInfo consumable)
        {
            if (!_inventoryController.Inventory.Remove(consumable)) return false;

            _inventoryController.Inventory.WalletController.Wallet.Gold.UpdateCurrencyAmount(consumable.SellPrice);

            return true;
        }
    }
}