using UnityEngine;

namespace CryptoQuest.Item.Inventory
{
    [CreateAssetMenu(menuName = "Crypto Quest/Inventory/Item")]
    public class ItemSO : ScriptableObject
    {
        public Sprite Icon;
        public string Name;
        public int Amount;
        public string Description;
    }
}