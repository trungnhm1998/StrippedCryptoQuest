using System;
using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Type;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization;

namespace CryptoQuest.Item
{
    [Serializable]
    public class ConsumableInfo : ItemInfo<ConsumableSO>
    {
        public static event Action<ConsumableInfo> QuantityReduced;

        [SerializeField] private int _quantity = 1;
        public int Quantity => _quantity;

        public AssetReferenceT<Sprite> Icon => Data.Image;
        public LocalizedString DisplayName => Data.DisplayName;
        public LocalizedString Description => Data.Description;

        public override int Price => Data.Price;
        public override int SellPrice => Data.SellPrice;

        public string DataId { get; private set; }

        public ConsumableInfo(ConsumableSO baseItemSO, int quantity = 1) : base(baseItemSO)
        {
            _quantity = quantity;
        }

        public ConsumableInfo(string dataId)
        {
            DataId = dataId;
        }

        public ConsumableInfo() { }

        public void SetQuantity(int quantity)
        {
            _quantity = quantity;
        }

        public void Consuming()
        {
            // I can pass this into the event
            Data.TargetSelectionEvent.RaiseEvent();
        }

        public void OnConsumed(IInventoryController inventoryController)
        {
            ReduceQuantityOnUsed(inventoryController);
        }

        private void ReduceQuantityOnUsed(IInventoryController inventoryController)
        {
            if (Data.ConsumableType == EConsumableType.Key) return;
            if (inventoryController.Remove(this))
            {
                QuantityReduced?.Invoke(this);
            }
        }

        public override ItemInfo Clone() => new ConsumableInfo(Data, Quantity);

        public override bool IsValid()
        {
            if (Data == null)
            {
                Debug.LogWarning("Consumable doesn't have Data");
                return false;
            }

            if (Quantity <= 0)
            {
                Debug.LogWarning($"Consumable {Data} is less than 0");
                return false;
            }

            return base.IsValid();
        }

        public override bool AddToInventory(IInventoryController inventoryController) =>
            inventoryController.Add(this, Quantity);

        public override bool RemoveFromInventory(IInventoryController inventory)
        {
            return false;
        }
    }
}