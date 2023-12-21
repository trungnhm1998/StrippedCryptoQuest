using System.Collections.Generic;
using System.Linq;
using CryptoQuest.Inventory.ScriptableObjects;
using IndiGames.Core.Events;
using UnityEngine;

namespace CryptoQuest.Sagas.Equipment
{
    public class UpdateEquipmentStonesAfterDetached : SagaBase<DetachSucceeded>
    {
        [SerializeField] private EquipmentInventory _inventory;

        protected override void HandleAction(DetachSucceeded ctx)
        {
            foreach (var equipment in _inventory.Equipments)
            {
                if (ctx.EquipmentID != equipment.Id) continue;
                equipment.Data.AttachStones.Remove(ctx.StoneIDs[0]);
            }
        }
    }
}