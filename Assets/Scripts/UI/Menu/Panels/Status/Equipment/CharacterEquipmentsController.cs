using CryptoQuest.Gameplay;
using CryptoQuest.Gameplay.Inventory.Items;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Container;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.System;
using UnityEngine;
using NotImplementedException = System.NotImplementedException;

namespace CryptoQuest.UI.Menu.Panels.Status.Equipment
{
    /// <summary>
    /// To manage the character's equipment, equip, unequip, etc.
    /// make sure this component awake first
    /// </summary>
    public class CharacterEquipmentsController : MonoBehaviour
    {
        [SerializeField] private ServiceProvider _provider;
        private IParty _party;

        private void Awake()
        {
            _provider.UnequipCharacterEquipmentAtSlot += UnequipEquipmentAtSlot;
            _provider.PartyProvided += RegisterMemberEquipmentEvents;
        }

        private void OnDestroy()
        {
            _provider.UnequipCharacterEquipmentAtSlot -= UnequipEquipmentAtSlot;
            _provider.PartyProvided -= RegisterMemberEquipmentEvents;

            RemoveMemberEquipmentEvents();
        }

        private void RegisterMemberEquipmentEvents(IPartyController partyController)
        {
            _party = partyController.Party;
            for (int i = 0; i < _party.Members.Length; i++)
            {
                var character = _party.Members[i];
                if (character.IsValid() == false) continue; // some slot can empty but not null

                character.Equipments.EquipmentAdded += RemoveEquipmentFromInventory;
                character.Equipments.EquipmentRemoved += AddEquipmentIntoInventory;
            }
        }

        private void RemoveMemberEquipmentEvents()
        {
            if (_party == null) return;

            for (int i = 0; i < _party.Members.Length; i++)
            {
                var character = _party.Members[i];
                if (character.IsValid() == false) continue;

                character.Equipments.EquipmentAdded -= RemoveEquipmentFromInventory;
                character.Equipments.EquipmentRemoved -= AddEquipmentIntoInventory;
            }
        }

        private void AddEquipmentIntoInventory(EquipmentInfo equipment) { }

        private void RemoveEquipmentFromInventory(EquipmentInfo equipment) { }

        private void UnequipEquipmentAtSlot(int charIndexInParty, EquipmentSlot.EType slotType)
        {
            if (!_provider.PartyController.TryGetMemberAtIndex(charIndexInParty, out ICharacter character)) return;

            var equipments = character.Spec.Equipments;
            var equipmentAtSlot = equipments.GetEquipmentInSlot(slotType);
            if (equipmentAtSlot.IsValid() == false) return;

            Debug.Log($"Unequip equipment {equipmentAtSlot} at slot {slotType}");
        }
    }
}