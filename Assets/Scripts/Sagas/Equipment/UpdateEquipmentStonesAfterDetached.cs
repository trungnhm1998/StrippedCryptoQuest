using System.Collections.Generic;
using System.Linq;
using CryptoQuest.Inventory.ScriptableObjects;
using CryptoQuest.Item.MagicStone;
using IndiGames.Core.Events;
using UnityEngine;

namespace CryptoQuest.Sagas.Equipment
{
    public class UpdateEquipmentStonesAfterDetached : SagaBase<DetachSucceeded>
    {
        [SerializeField] private MagicStoneInventory _stoneInventory;
        [SerializeField] private EquipmentInventory _inventory;

        protected override void HandleAction(DetachSucceeded ctx)
        {
            UpdateStone(ctx);
            
            foreach (var equipment in _inventory.Equipments)
            {
                if (ctx.EquipmentID != equipment.Id) continue;
                equipment.Data.AttachStones.Remove(ctx.StoneIDs[0]);
                ActionDispatcher.Dispatch(new EquipmentUpdated(equipment));
            }
        }

        private void UpdateStone(DetachSucceeded ctx)
        {
            foreach (var stoneId in ctx.StoneIDs)
            {
                var stone = _stoneInventory.GetStone(stoneId);
                if (!stone.IsValid()) continue;
                stone.AttachEquipmentId = 0;
            }
        }
    }
}