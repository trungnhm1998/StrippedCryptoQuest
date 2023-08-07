using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Type;
using IndiGames.GameplayAbilitySystem.AbilitySystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item
{
    [CreateAssetMenu(fileName = "Usable Item", menuName = "Crypto Quest/Inventory/Usable Item")]
    public class UsableSO : ItemGenericSO
    {
        [field: Header("Usable Item")]
        [field: SerializeField] public UsableTypeSO UsableTypeSO { get; private set; }
        [field: SerializeField] public AbilityScriptableObject Ability { get; private set; }
    }
}