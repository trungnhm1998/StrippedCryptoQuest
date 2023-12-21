using System.Collections.Generic;
using CryptoQuest.Inventory.ScriptableObjects;
using IndiGames.Core.Events;
using UnityEngine;

namespace CryptoQuest.Sagas.Equipment
{
    public class UpdateEquipmentStonesAfterAttached : SagaBase<AttachSucceeded>
    {
        [SerializeField] private EquipmentInventory _inventory;

        protected override void HandleAction(AttachSucceeded ctx)
        {
            foreach (var equipment in _inventory.Equipments)
            {
                if (ctx.EquipmentID == equipment.Id)
                    equipment.Data.AttachStones = ctx.StoneIDs;
            }
        }
    }
}