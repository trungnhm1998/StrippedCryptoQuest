using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.Item.Equipment;
using System.Collections.Generic;

namespace CryptoQuest.Gameplay.PlayerParty.Helper
{
    public static class PartyExtensions
    {
        public static IEnumerable<EquipmentInfo> GetEquippedEquipments(this IPartyController party)
        {
            foreach (var member in party.Slots)
            {
                if (!member.IsValid()) continue;
                member.HeroBehaviour.Spec.ValidateEquipments();
                foreach (var slot in member.HeroBehaviour.Spec.Equipments.Slots)
                {
                    if (!slot.IsValid()) continue;
                    yield return slot.Equipment;
                }
            }
        }
    }
}