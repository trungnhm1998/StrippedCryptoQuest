using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Type;
using IndiGames.GameplayAbilitySystem.AbilitySystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item
{
    [CreateAssetMenu(fileName = "Usable Item", menuName = "Crypto Quest/Inventory/Usable Item")]
    public class UsableSO : ItemGenericSO
    {
        [Header("Usable Item")]
        [SerializeField] private UsableTypeSO usableTypeSO;

        public UsableTypeSO UsableTypeSO
        {
            get => usableTypeSO;
            set => usableTypeSO = value;
        }

        [field: SerializeField] public AbilityScriptableObject Ability { get; private set; }
    }
}