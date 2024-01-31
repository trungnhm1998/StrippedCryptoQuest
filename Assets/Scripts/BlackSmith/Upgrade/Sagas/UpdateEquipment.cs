using CryptoQuest.Battle.Components;
using CryptoQuest.BlackSmith.Upgrade.Actions;
using CryptoQuest.Gameplay.PlayerParty;
using IndiGames.Core.Events;
using UnityEngine;

namespace CryptoQuest.BlackSmith.Upgrade.Sagas
{
    public class UpdateEquipment : SagaBase<UpgradeSucceed>
    {
        private IPartyController _partyController;

        protected override void HandleAction(UpgradeSucceed ctx)
        {
            var info = ctx.UpgradedEquipmentInfo;
            var hero = info.EquippedHero;
            if (hero == null || !hero.IsValid())
            {
                ctx.UpgradedEquipmentInfo.Equipment.Level = ctx.Level;
                return;
            }

            var equipmentController = hero.GetComponent<EquipmentsController>();
            // Level should be update after unequip because if I update its first the controller
            // cannot find the effect from equipment to remove
            info.Equipment.Level = ctx.Level;  
            equipmentController.Equip(info.Equipment, info.Slot);
        }
    }
}