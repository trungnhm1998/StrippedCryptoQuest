using CryptoQuest.Battle.Components;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.Inventory.Actions;
using CryptoQuest.Item.Equipment;
using IndiGames.Core.Common;
using IndiGames.Core.Events;

namespace CryptoQuest.BlackSmith.Evolve.Sagas
{
    public class RemoveEquipmentsSaga : SagaBase<RemoveEquipments>
    {
        private IPartyController _partyController;

        protected override void HandleAction(RemoveEquipments ctx)
        {
            foreach (var equipment in ctx.Equipments)
            {
                if (equipment == null || !equipment.IsValid()) return;
                ActionDispatcher.Dispatch(new RemoveEquipmentAction(equipment));
                UpdateEquipping(equipment);
            }
        }

        private void UpdateEquipping(IEquipment equipment)
        {
            _partyController ??= ServiceProvider.GetService<IPartyController>();
            
            foreach (var slot in _partyController.Slots)
            {
                if (!slot.IsValid()) continue;

                var hero = slot.HeroBehaviour;
                foreach (var equipSlot in hero.GetEquipments().Slots)
                {
                    var equipping = equipSlot.Equipment;
                    if (equipping == null || !equipping.IsValid() || !equipping.Equals(equipment))
                        continue;
                    
                    var equipmentController = hero.GetComponent<EquipmentsController>();
                    equipmentController.Unequip(equipSlot.Type);
                    ActionDispatcher.Dispatch(new RemoveEquipmentAction(equipment));
                    return;
                }
            }
        }
    }
}
