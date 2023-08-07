using IndiGames.GameplayAbilitySystem.AbilitySystem.ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;

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
        [Header("Usable Item")] public UsableTypeSO UsableTypeSO;

        [SerializeField] private EItemType _itemType;

        public AbilityScriptableObject Ability;
    }
}