using System.Collections.Generic;
using System.Linq;
using CryptoQuest.Inventory.ScriptableObjects;
using IndiGames.Core.Events;
using TinyMessenger;
using UnityEngine;

namespace CryptoQuest.Sagas.Equipment
{
    public class UpdateEquipmentStonesAfterDetached : SagaBase<DetachStones>
    {
        [SerializeField] private EquipmentInventory _inventory;

        private TinyMessageSubscriptionToken _detachSucceededToken;
        private int _equipmentID;
        private List<int> _stoneIDs;

        protected override void OnEnable()
        {
            base.OnEnable();
            _detachSucceededToken = ActionDispatcher.Bind<DetachSucceeded>(UpdateInventory);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            ActionDispatcher.Unbind(_detachSucceededToken);
        }

        protected override void HandleAction(DetachStones ctx)
        {
            _equipmentID = ctx.EquipmentID;
            _stoneIDs = ctx.StoneIDs;
        }

        private void UpdateInventory(DetachSucceeded _)
        {
            List<int> attachStones = new();
            foreach (var equipment in _inventory.Equipments)
            {
                if (_equipmentID != equipment.Id) continue;
                attachStones = equipment.Data.AttachStones.ToList();
                attachStones.Remove(_equipmentID);
                equipment.Data.AttachStones = attachStones.ToArray();
            }
        }
    }
}