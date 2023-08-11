using CryptoQuest.Events.Gameplay;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using UnityEngine;

namespace CryptoQuest.Gameplay.Inventory
{
    public class InventoryController : MonoBehaviour
    {
        [SerializeField] private InventorySO _inventorySO;
        [SerializeField] private AbilitySystemBehaviour CurrentOwnerAbilitySystemBehaviour;

        [Header("Events listening")]
        [SerializeField] private ItemEventChannelSO _onUseItem;

        private void OnEnable()
        {
            _onUseItem.EventRaised += UseItem;
        }

        private void OnDisable()
        {
            _onUseItem.EventRaised -= UseItem;
        }

        private void UseItem(UsableInfo item)
        {
            if (item == null) return;
            item.Owner = CurrentOwnerAbilitySystemBehaviour;
            item.UseItem();
        }
    }
}