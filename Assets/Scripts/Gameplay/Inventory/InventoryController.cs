using CryptoQuest.Events.Gameplay;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using UnityEngine;

namespace CryptoQuest.Gameplay.Inventory
{
    public class InventoryController : MonoBehaviour
    {
        [SerializeField] private InventorySO _inventorySO;
        [SerializeField] private ItemEventChannelSO _onEquipItemEventChannel;
        [SerializeField] private AbilitySystemBehaviour CurrentOwnerAbilitySystemBehaviour;


        private void OnEnable()
        {
            _onEquipItemEventChannel.EventRaised += EquipItem;
        }

        private void OnDisable()
        {
            _onEquipItemEventChannel.EventRaised -= EquipItem;
        }

        private void EquipItem(UsableInformation item)
        {
            if (item == null) return;
            item.Owner = CurrentOwnerAbilitySystemBehaviour;
            item.UseItem();
        }
    }
}