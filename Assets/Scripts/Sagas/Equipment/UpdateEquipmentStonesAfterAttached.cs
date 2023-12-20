using System.Collections.Generic;
using CryptoQuest.Inventory.ScriptableObjects;
using IndiGames.Core.Events;
using TinyMessenger;
using UnityEngine;

namespace CryptoQuest.Sagas.Equipment
{
    public class UpdateEquipmentStonesAfterAttached : SagaBase<AttachStones>
    {
        [SerializeField] private EquipmentInventory _inventory;

        private TinyMessageSubscriptionToken _attachSucceededToken;
        private int _equipmentID;
        private List<int> _stoneIDs;

        protected override void OnEnable()
        {
            base.OnEnable();
            _attachSucceededToken = ActionDispatcher.Bind<AttachSucceeded>(UpdateInventory);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            ActionDispatcher.Unbind(_attachSucceededToken);
        }

        protected override void HandleAction(AttachStones ctx)
        {
            _equipmentID = ctx.EquipmentID;
            _stoneIDs = ctx.StoneIDs;
        }

        private void UpdateInventory(AttachSucceeded _)
        {
            foreach (var equipment in _inventory.Equipments)
            {
                if (_equipmentID == equipment.Id)
                    equipment.Data.AttachStones = _stoneIDs;
            }
        }
    }
}