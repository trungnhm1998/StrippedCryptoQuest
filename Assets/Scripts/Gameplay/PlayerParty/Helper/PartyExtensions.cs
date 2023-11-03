using System.Collections.Generic;
using CryptoQuest.Item.Equipment;

namespace CryptoQuest.Gameplay.PlayerParty.Helper
{
    public static class PartyExtensions
    {
        public static IEnumerable<EquipmentInfo> GetEquippedEquipments(this IPartyController party)
        {
            foreach (var partySlot in party.Slots)
            {
                if (!partySlot.IsValid()) continue;
                partySlot.Spec.ValidateEquipments();
                foreach (var equipmentSlot in partySlot.Spec.EquippingItems.Slots)
                {
                    if (!equipmentSlot.IsValid()) continue;
                    yield return equipmentSlot.Equipment;
                }
            }
        }
    }
}