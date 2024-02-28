using CryptoQuest.Battle.Components;
using CryptoQuest.BlackSmith.Upgrade.Actions;
using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.Item.Equipment;
using IndiGames.Core.Common;
using IndiGames.Core.Events;

namespace CryptoQuest.BlackSmith.Upgrade.Sagas
{
    public class UpgradeResponseHandle : SagaBase<UpgradeResponsed>
    {
        private IInventoryController _inventoryController;
        private IPartyController _partyController;

        private HeroBehaviour _hero;
        private ESlot _slotType;

        protected override void HandleAction(UpgradeResponsed ctx)
        {
            var gold = ctx.Response.gold;
            var level = ctx.Response.data.equipment.lv;
            var equipmentId = ctx.Response.data.equipment.id;

            _inventoryController ??= ServiceProvider.GetService<IInventoryController>();

            var equipment = TryFindEquipment(equipmentId);

            if (!equipment.IsValid())
            {
                ActionDispatcher.Dispatch(new UpgradeFailed());
                return;
            }
            
            var equipmentInfo = new UpgradedEquipmentInfo()
            {
                Equipment = equipment,
                EquippedHero = _hero,
                Slot = _slotType
            };

            ActionDispatcher.Dispatch(new UpgradeSucceed(equipmentInfo, level, gold));
        }

        private IEquipment TryFindEquipment(int id)
        {
            _inventoryController ??= ServiceProvider.GetService<IInventoryController>();

            var equipment = _inventoryController.Inventory.Equipments.Find(e => e.Id == id);
            if (equipment != null) return equipment;

            var equippingEquipment = TryFindEquipping(id);
            if (equippingEquipment != null) return equippingEquipment;

            return new Equipment();
        }

        private IEquipment TryFindEquipping(int id)
        {
            _partyController ??= ServiceProvider.GetService<IPartyController>();

            foreach (var slot in _partyController.Slots)
            {
                if (!slot.IsValid()) continue;
                var hero = slot.HeroBehaviour;

                foreach (var equipSlot in hero.GetEquipments().Slots)
                {
                    var equipping = equipSlot.Equipment;
                    if (!equipping.IsValid() || equipping.Id != id) continue;
                    _hero = hero;
                    _slotType = equipSlot.Type;
                    return equipping;
                }
            }

            return new Equipment();
        }
    }
}