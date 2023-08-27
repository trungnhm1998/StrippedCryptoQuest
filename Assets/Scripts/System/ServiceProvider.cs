using System;
using CryptoQuest.Gameplay.Character;
using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.Gameplay.Inventory.Items;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Container;
using CryptoQuest.Gameplay.PlayerParty;
using UnityEngine;

namespace CryptoQuest.System
{
    /// <summary>
    /// Instead of using Event that could raise on multiple instances, I only want 1 instance 
    /// Act as service locator for the inventory, party and equipment controllers
    ///
    /// The safest way to work with this is create a scene where we know these services are available
    /// inject into this provider
    ///
    /// Then load the next scene where we need these services
    ///
    /// This is an anti-pattern, every time a new service is added, we need to update this provider
    /// TODO: refactor to use dependency injection
    /// </summary>
    public class ServiceProvider : ScriptableObject
    {
        public delegate void EquipmentWithSlotHandler(CharacterSpec charIndexInParty,
            EquipmentSlot.EType slotType);

        public event EquipmentWithSlotHandler UnequipCharacterEquipmentAtSlot;
        public event Action<CharacterSpec, EquipmentInfo> EquipCharacterEquipmentAtSlot;
        public event Action<IPartyController> PartyProvided;
        public IInventoryController InventoryController { get; private set; }

        public IPartyController PartyController { get; private set; }

        public IInventory Inventory { get; private set; }

        private void ProvideBase(object service)
        {
            Debug.Log($"Bind service [{service.GetType().Name}] to {name}");
        }

        public void Provide(IInventoryController service)
        {
            ProvideBase(service);
            InventoryController = service;
        }

        public void Provide(IPartyController service)
        {
            ProvideBase(service);
            PartyController = service;
            PartyProvided?.Invoke(service);
        }

        public void Provide(IInventory service)
        {
            ProvideBase(service);
            Inventory = service;
        }

        public void UnequipEquipmentAtSlot(CharacterSpec inspectingChar, EquipmentSlot.EType slotType)
        {
            UnequipCharacterEquipmentAtSlot?.Invoke(inspectingChar, slotType);
        }

        public void EquipEquipment(CharacterSpec inspectingCharacter, EquipmentInfo equipment)
        {
            EquipCharacterEquipmentAtSlot?.Invoke(inspectingCharacter, equipment);
        }
    }
}