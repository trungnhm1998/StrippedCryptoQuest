using CryptoQuest.Data.Item;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Gameplay.Inventory.ScriptableObjects
{
    public enum EItemType
    {
        Expendables = 0,
        Valuables = 1,
        NFT = 2
    }

    [CreateAssetMenu(menuName = "Crypto Quest/Inventory/Item")]
    public class ItemSO : ScriptableObject
    {
        public EItemType Type;
        public Sprite Icon;
        public LocalizedString Name;
        public LocalizedString Description;
    }
}