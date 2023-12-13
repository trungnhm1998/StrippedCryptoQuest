using System.Collections.Generic;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.Menus.DimensionalBox.States.EquipmentsTransfer;
using CryptoQuest.Menus.DimensionalBox.UI.EquipmentsTransfer;
using IndiGames.Core.Events;
using TinyMessenger;
using UnityEngine;

namespace CryptoQuest.Menus.DimensionalBox.Sagas
{
    public class UpdateInventoryOnTransferred : SagaBase<TransferringEquipments>
    {
        [SerializeField] private InventorySO _inventory;

        private List<UIEquipment> _equipmentsToRemove;
        private List<UIEquipment> _equipmentsToAdd;
        private TinyMessageSubscriptionToken _transferSucceedEvent;

        protected override void OnEnable()
        {
            base.OnEnable();
            _transferSucceedEvent = ActionDispatcher.Bind<TransferSucceed>(UpdateInventory);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            ActionDispatcher.Unbind(_transferSucceedEvent);
        }

        protected override void HandleAction(TransferringEquipments ctx)
        {
            _equipmentsToRemove = ctx.ToWallet;
            _equipmentsToAdd = ctx.ToGame;
        }

        private void UpdateInventory(TransferSucceed ctx)
        {
            Dictionary<int, int> idToIndexCache = new();
            for (int i = 0; i < _inventory.Equipments.Count; i++) idToIndexCache.Add(_inventory.Equipments[i].Id, i);

            foreach (var ui in _equipmentsToRemove) _inventory.Equipments.RemoveAt(idToIndexCache[ui.Id]);
            foreach (var ui in _equipmentsToAdd) _inventory.Equipments.Add(ui.Equipment);
        }
    }
}