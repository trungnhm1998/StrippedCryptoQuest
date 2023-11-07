using CryptoQuest.Core;
using CryptoQuest.Menus.DimensionalBox.UI;
using TinyMessenger;
using UnityEngine;

namespace CryptoQuest.Menus.DimensionalBox.States
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
            foreach (var equipmentList in _equipmentLists)
            {
                equipmentList.Initialized += FocusOnFirstInitializedList;
                equipmentList.Transferring += TransferEquipmentToOtherListAndFocus;
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
            foreach (var equipmentList in _equipmentLists)
            {
                equipmentList.Initialized -= FocusOnFirstInitializedList;
                equipmentList.Transferring -= TransferEquipmentToOtherListAndFocus;
            }
        }

        private void RegisterEscapeKey(ActionBase _) => StateMachine.Input.MenuCancelEvent += ToSelectTransferTypeState;

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