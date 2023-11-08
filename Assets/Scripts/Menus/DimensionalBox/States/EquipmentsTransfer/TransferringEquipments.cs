using System.Linq;
using CryptoQuest.Battle.Components;
using CryptoQuest.Core;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.Menus.DimensionalBox.UI.EquipmentsTransfer;
using CryptoQuest.Sagas.Objects;
using CryptoQuest.System;
using TinyMessenger;
using UnityEngine;

namespace CryptoQuest.Menus.DimensionalBox.States.EquipmentsTransfer
{
    internal class TransferringEquipments : StateBase
    {
        private readonly GameObject _equipmentsTransferPanel;
        private UIEquipmentList[] _equipmentLists;

        public TransferringEquipments(GameObject equipmentsTransferPanel)
        {
            _equipmentsTransferPanel = equipmentsTransferPanel;
            _equipmentLists = _equipmentsTransferPanel.GetComponentsInChildren<UIEquipmentList>();
        }

        private TinyMessageSubscriptionToken _getDataSucceed;
        private TinyMessageSubscriptionToken _getDataFailed;

        protected override void OnEnter()
        {
            _equipmentsTransferPanel.SetActive(true);
            _hasFocusOnFirstInitializedList = false;
            _getDataSucceed = ActionDispatcher.Bind<GetNftEquipmentsSucceed>(RegisterEscapeKey);
            _getDataFailed = ActionDispatcher.Bind<GetNftEquipmentsFailed>(RegisterEscapeKey);
            StateMachine.Input.MenuExecuteEvent += ConfirmTransfer;
            StateMachine.Input.MenuNavigateEvent += MoveBetweenList;
            StateMachine.Input.MenuResetEvent += ResetToOriginals;
            foreach (var equipmentList in _equipmentLists)
            {
                equipmentList.Initialized += FocusOnFirstInitializedList;
                equipmentList.Transferring += TransferEquipmentToOtherListAndFocus;
                if (_hasFocusOnFirstInitializedList || !equipmentList.Focus()) continue;
                _hasFocusOnFirstInitializedList = true;
            }

            ActionDispatcher.Dispatch(new GetNftEquipments());
        }

        protected override void OnExit()
        {
            _hasFocusOnFirstInitializedList = false;
            ActionDispatcher.Unbind(_getDataSucceed);
            ActionDispatcher.Unbind(_getDataFailed);
            StateMachine.Input.MenuCancelEvent -= ToSelectTransferTypeState;
            StateMachine.Input.MenuExecuteEvent -= ConfirmTransfer;
            StateMachine.Input.MenuNavigateEvent -= MoveBetweenList;
            StateMachine.Input.MenuResetEvent -= ResetToOriginals;
            foreach (var equipmentList in _equipmentLists)
            {
                equipmentList.Initialized -= FocusOnFirstInitializedList;
                equipmentList.Transferring -= TransferEquipmentToOtherListAndFocus;
            }
        }

        private void ResetToOriginals()
        {
            _hasFocusOnFirstInitializedList = false;
            foreach (var equipmentList in _equipmentLists) equipmentList.Reset();
        }

        private void RegisterEscapeKey(ActionBase _)
        {
            StateMachine.Input.MenuCancelEvent += ToSelectTransferTypeState;
            UpdateEquipmentsEquippingState();
        }

        private void UpdateEquipmentsEquippingState()
        {
            var inGameEquipments = _equipmentLists[0].ScrollView.content.GetComponentsInChildren<UIEquipment>();
            foreach (var equipmentUI in inGameEquipments)
                equipmentUI.EquippedTag.SetActive(IsEquipping(equipmentUI.Equipment));

            inGameEquipments = _equipmentLists[1].ScrollView.content.GetComponentsInChildren<UIEquipment>();
            foreach (var equipmentUI in inGameEquipments)
            {
                equipmentUI.EquippedTag.SetActive(IsEquipping(equipmentUI.Equipment));
                if (equipmentUI.EquippedTag.activeSelf == false) continue;
                _equipmentLists[1].OnTransferring(equipmentUI);
            }
        }

        private IPartyController _partyManager;

        private bool IsEquipping(EquipmentResponse equipmentResponse)
        {
            _partyManager ??= ServiceProvider.GetService<IPartyController>();
            foreach (var slot in _partyManager.Slots)
            {
                if (slot.IsValid() == false) continue;
                slot.HeroBehaviour.TryGetComponent(out EquipmentsController equipmentsController);
                var equipmentsSlots = equipmentsController.Equipments.Slots;
                if (equipmentsSlots.Any(equipmentSlot => equipmentSlot.Equipment.Id == equipmentResponse.id))
                {
                    return true;
                }
            }

            return false;
        }

        private void ToSelectTransferTypeState()
        {
            _equipmentsTransferPanel.SetActive(false);
            StateMachine.ChangeState(StateMachine.Landing);
        }

        private void ConfirmTransfer()
        {
            if (_equipmentLists[0].PendingTransfer == false && _equipmentLists[1].PendingTransfer == false) return;
            StateMachine.ChangeState(StateMachine.ConfirmTransfer);
        }

        private bool _hasFocusOnFirstInitializedList = false;
        private UIEquipmentList _focusingList;

        private void FocusOnFirstInitializedList(UIEquipmentList list)
        {
            if (_hasFocusOnFirstInitializedList) return;
            if (FocusOnEquipmentList(list))
                _hasFocusOnFirstInitializedList = true;
        }

        /// <summary>
        /// Move between 2 list, order based on Scene Hierarchy
        /// </summary>
        /// <param name="dir"></param>
        private void MoveBetweenList(Vector2 dir)
        {
            if (_hasFocusOnFirstInitializedList == false) return;
            switch (dir.x)
            {
                case < 0:
                    FocusOnEquipmentList(_equipmentLists[0]);
                    break;
                case > 0:
                    FocusOnEquipmentList(_equipmentLists[1]);
                    break;
            }

            _focusingList.Navigate(dir.y * -1);
        }

        private bool FocusOnEquipmentList(UIEquipmentList list)
        {
            var toTop = _focusingList == list;
            if (!list.Focus(toTop)) return false;
            _focusingList = list;
            return true;
        }

        private void TransferEquipmentToOtherListAndFocus(UIEquipment equipment)
        {
            var otherList = _equipmentLists[0] == _focusingList ? _equipmentLists[1] : _equipmentLists[0];
            otherList.Transfer(equipment);
            _focusingList = otherList;
        }
    }
}