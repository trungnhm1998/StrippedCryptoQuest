using System;
using CryptoQuest.Events.Gameplay;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Container;
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

        [SerializeField] private EquipmentEventChannelSO _onEquipItem;
        [SerializeField] private EquipmentEventChannelSO _onUnequipItem;

        private void OnEnable()
        {
            _onEquipItem.EventRaised += Equip;
            _onUnequipItem.EventRaised += Unequip;

            _onUseItem.EventRaised += UseItem;
            _onAddItem.EventRaised += AddItem;

            _onRemoveItem.EventRaised += RemoveItem;
        }

        private void OnDisable()
        {
            _onEquipItem.EventRaised -= Equip;
            _onUnequipItem.EventRaised -= Unequip;

            _onUseItem.EventRaised -= UseItem;
            _onAddItem.EventRaised -= AddItem;

            _onRemoveItem.EventRaised -= RemoveItem;
        }

        private void Equip(EquippingSlotContainer.EType slot, EquipmentInfo equipment)
        {
            _inventorySO.Equip(slot, equipment);
        }

        private void Unequip(EquippingSlotContainer.EType slot, EquipmentInfo equipment)
        {
            _inventorySO.Unequip(slot);
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