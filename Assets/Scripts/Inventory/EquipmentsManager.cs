using CryptoQuest.Inventory.Actions;
using CryptoQuest.Inventory.ScriptableObjects;
using IndiGames.Core.Events;
using TinyMessenger;
using UnityEngine;

namespace CryptoQuest.Inventory
{
    public class EquipmentsManager : MonoBehaviour
    {
        [SerializeField] private EquipmentInventory _equipmentInventory;
        private TinyMessageSubscriptionToken _addEquipmentEvent;
        private TinyMessageSubscriptionToken _removeEquipmentEvent;

        private void OnEnable()
        {
            _addEquipmentEvent = ActionDispatcher.Bind<AddEquipmentAction>(AddEquipment);
            _removeEquipmentEvent = ActionDispatcher.Bind<RemoveEquipmentAction>(RemoveEquipment);
        }

        private void AddEquipment(AddEquipmentAction ctx)
        {
            _equipmentInventory.Add(ctx.Item);
        }

        private void RemoveEquipment(RemoveEquipmentAction ctx)
        {
            _equipmentInventory.Remove(ctx.Item);
        }

        private void OnDisable()
        {
            ActionDispatcher.Unbind(_addEquipmentEvent);
            ActionDispatcher.Unbind(_removeEquipmentEvent);
        }
    }
}