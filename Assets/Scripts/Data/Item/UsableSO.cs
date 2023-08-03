using UnityEngine;

namespace CryptoQuest.Data.Item
{
    public enum EItemType
    {
        Key = 0,
        Consumable = 1,
    }

    [CreateAssetMenu(fileName = "Usable Item", menuName = "Crypto Quest/Inventory/Usable Item")]
    public class UsableSO : ItemGenericSO
    {
        [SerializeField] private UsableTypeSo _usableTypeSo;
        [SerializeField] private EItemType _itemType;
    }
}