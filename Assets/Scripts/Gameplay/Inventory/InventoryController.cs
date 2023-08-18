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

        [SerializeField] private ItemEventChannelSO _onAddItem;
        [SerializeField] private ItemEventChannelSO _onRemoveItem;

        private void OnEnable()
        {
            _onUseItem.EventRaised += UseItem;
            _onAddItem.EventRaised += AddItem;

            _onRemoveItem.EventRaised += RemoveItem;
        }

        private void OnDisable()
        {
            _onUseItem.EventRaised -= UseItem;
            _onAddItem.EventRaised -= AddItem;

            _onRemoveItem.EventRaised -= RemoveItem;
        }


        private void AddItem(UsableInfo item)
        {
            _inventorySO.Add(item);
        }

        private void RemoveItem(UsableInfo item)
        {
            _inventorySO.Remove(item);
        }

        private void UseItem(UsableInfo item)
        {
            if (item == null) return;
            item.Owner = CurrentOwnerAbilitySystemBehaviour;
            item.UseItem();
        }
    }
}