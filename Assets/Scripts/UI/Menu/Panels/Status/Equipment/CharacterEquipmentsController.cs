using UnityEngine;

namespace CryptoQuest.UI.Menu.Panels.Status.Equipment
{
    // TODO: REFACTOR PARTY
    /// <summary>
    /// To manage a character's equipment, equip, unequip, etc. in the party
    /// </summary>
    public class CharacterEquipmentsController : MonoBehaviour
    {
        // private IParty _party;
        // private bool _bindThroughEvent;
        //
        // private void Awake()
        // {
        //     _provider.UnequipCharacterEquipmentAtSlot += UnequipEquipmentAtSlot;
        //     _provider.EquipCharacterEquipmentAtSlot += EquipItem;
        //     if (_provider.PartyController != null)
        //         RegisterMemberEquipmentEvents(_provider.PartyController);
        //     else
        //     {
        //         _bindThroughEvent = true;
        //         _provider.PartyProvided += RegisterMemberEquipmentEvents;
        //     }
        // }
        //
        // private void OnDestroy()
        // {
        //     if (_bindThroughEvent)
        //         _provider.PartyProvided -= RegisterMemberEquipmentEvents;
        //     _provider.UnequipCharacterEquipmentAtSlot -= UnequipEquipmentAtSlot;
        //     _provider.EquipCharacterEquipmentAtSlot -= EquipItem;
        //     RemoveMemberEquipmentEvents();
        // }
        //
        // private void RegisterMemberEquipmentEvents(IPartyController partyController)
        // {
        //     _party = partyController.Party;
        //     for (int i = 0; i < _party.Members.Length; i++)
        //     {
        //         var character = _party.Members[i];
        //         if (character.IsValid() == false) continue; // some slot can empty but not null
        //
        //         character.Equipments.EquipmentAdded += RemoveEquipmentFromInventory;
        //         character.Equipments.EquipmentRemoved += AddEquipmentIntoInventory;
        //     }
        // }
        //
        // private void RemoveMemberEquipmentEvents()
        // {
        //     if (_party == null) return;
        //
        //     for (int i = 0; i < _party.Members.Length; i++)
        //     {
        //         var character = _party.Members[i];
        //         if (character.IsValid() == false) continue;
        //
        //         character.Equipments.EquipmentAdded -= RemoveEquipmentFromInventory;
        //         character.Equipments.EquipmentRemoved -= AddEquipmentIntoInventory;
        //     }
        // }
        //
        // private void EquipItem(CharacterSpec character, EquipmentInfo equipment)
        // {
        //     if (character.IsValid() == false)
        //     {
        //         Debug.LogWarning($"CharacterEquipmentsController::EquipItem: No character is inspecting");
        //         return;
        //     }
        //
        //     if (equipment.IsValid() == false)
        //     {
        //         Debug.LogWarning($"CharacterEquipmentsController::EquipItem: No equipment is selected");
        //         return;
        //     }
        //
        //     character.Equipments.Equip(equipment);
        // }
        //
        // private void RemoveEquipmentFromInventory(EquipmentInfo equipment, List<EquipmentSlot.EType> eTypes)
        // {
        //     if (equipment.IsValid() == false)
        //     {
        //         Debug.LogWarning(
        //             $"CharacterEquipmentsController::RemoveEquipmentFromInventory: No equipment is selected");
        //         return;
        //     }
        //
        //     _provider.InventoryController.Remove(equipment);
        // }
        //
        // private void UnequipEquipmentAtSlot(CharacterSpec characterSpec, EquipmentSlot.EType slotType)
        // {
        //     if (characterSpec.IsValid() == false)
        //     {
        //         Debug.LogWarning($"CharacterEquipmentsController::UnequipEquipmentAtSlot: No character is inspecting");
        //         return;
        //     }
        //
        //     var equipments = characterSpec.Equipments;
        //     var equipmentAtSlot = equipments.GetEquipmentInSlot(slotType);
        //     if (equipmentAtSlot.IsValid() == false)
        //     {
        //         Debug.Log($"CharacterEquipmentsController::UnequipEquipmentAtSlot: No equipment at slot {slotType}");
        //         return;
        //     }
        //
        //     characterSpec.Equipments.Unequip(equipmentAtSlot);
        //     Debug.Log($"Unequip equipment {equipmentAtSlot} at slot {slotType}");
        // }
        //
        // private void AddEquipmentIntoInventory(EquipmentInfo equipment, List<EquipmentSlot.EType> eTypes)
        // {
        //     _provider.InventoryController.Add(equipment);
        // }
    }
}