using System.Collections.Generic;
using CryptoQuest.Item.Equipment;

namespace CryptoQuest.Gameplay.PlayerParty.Helper
{
    public static class PartyExtension
    {
        public static List<IEquipment> GetEquippingEquipments(this IPartyController partyController)
        {
            List<IEquipment> equipments = new();

            foreach (var slot in partyController.Slots)
            {
                if (!slot.IsValid()) continue;

                var hero = slot.HeroBehaviour;
                foreach (var equipSlot in hero.GetEquipments().Slots)
                {
                    var equipping = equipSlot.Equipment;
                    if (equipping == null || !equipping.IsValid())
                        continue;

                    // Prevent add dual weilding
                    if (!equipments.Contains(equipping))
                        equipments.Add(equipping);
                }
            }

            return equipments;
        }
    }
}