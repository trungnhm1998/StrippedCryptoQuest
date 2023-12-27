using CryptoQuest.Inventory.Actions;
using CryptoQuest.Inventory.ScriptableObjects;
using CryptoQuest.Item.MagicStone;
using IndiGames.Core.Events;
using UnityEngine;
using MagicStoneItem = CryptoQuest.Item.MagicStone.MagicStone;

namespace CryptoQuest.Sagas.Equipment
{
    public class UpdateEquipmentStonesAfterAttached : SagaBase<AttachSucceeded>
    {
        [SerializeField] private MagicStoneInventory _stoneInventory;
        [SerializeField] private EquipmentInventory _inventory;

        protected override void HandleAction(AttachSucceeded ctx)
        {
            UpdateStone(ctx);

            foreach (var equipment in _inventory.Equipments)
            {
                if (ctx.EquipmentID != equipment.Id) continue;
                equipment.Data.AttachStones = ctx.StoneIDs;
                ActionDispatcher.Dispatch(new EquipmentUpdated(equipment));
            }
        }

        private void UpdateStone(AttachSucceeded ctx)
        {
            foreach (var stoneId in ctx.StoneIDs)
            {
                var stone = _stoneInventory.GetStone(stoneId);
                if (!stone.IsValid()) continue;
                stone.AttachEquipmentId = ctx.EquipmentID;
            }
        }
    }
}