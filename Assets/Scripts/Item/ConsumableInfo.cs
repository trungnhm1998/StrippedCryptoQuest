using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization;

namespace CryptoQuest.Item
{
    [Serializable]
    public class ConsumableInfo : ItemInfo<ConsumableSO>
    {
        [field: SerializeField] public int Quantity { get; private set; } = 1;

        public AssetReferenceT<Sprite> Icon => Data.Image;
        public LocalizedString DisplayName => Data.DisplayName;
        public LocalizedString Description => Data.Description;

        public ConsumableInfo(ConsumableSO baseItemSO, int quantity = 1) : base(baseItemSO)
        {
            Quantity = quantity;
        }

        public ConsumableInfo() { }

        public void SetQuantity(int quantity)
        {
            Quantity = quantity;
        }

        public void Consuming()
        {
            // I can pass this into the event
            Data.TargetSelectionEvent.RaiseEvent();
        }

        public ConsumableInfo Clone()
        {
            return new ConsumableInfo(Data, Quantity);
        }

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
    }
}