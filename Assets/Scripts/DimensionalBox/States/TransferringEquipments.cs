using System.Collections.Generic;
using CryptoQuest.Core;
using CryptoQuest.DimensionalBox.UI;
using UnityEngine;

namespace CryptoQuest.DimensionalBox.States
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

        protected override void OnEnter()
        {
            ActionDispatcher.Dispatch(new GetNftEquipments());
            _equipmentsTransferPanel.SetActive(true);
            StateMachine.Input.MenuCancelEvent += ToSelectTransferTypeState;
            StateMachine.Input.MenuNavigateEvent += MoveBetweenList;
            foreach (var equipmentList in _equipmentLists)
            {
                equipmentList.Initialized += FocusOnFirstInitializedList;
                equipmentList.Transferring += TransferEquipmentToOtherListAndFocus;
            }
        }

        protected override void OnExit()
        {
            _equipmentsTransferPanel.SetActive(false);
            StateMachine.Input.MenuCancelEvent -= ToSelectTransferTypeState;
            StateMachine.Input.MenuNavigateEvent -= MoveBetweenList;
            foreach (var equipmentList in _equipmentLists)
            {
                equipmentList.Initialized -= FocusOnFirstInitializedList;
                equipmentList.Transferring -= TransferEquipmentToOtherListAndFocus;
            }
        }

        private void ToSelectTransferTypeState() => StateMachine.ChangeState(StateMachine.Landing);

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

        private void FocusOnEquipmentList(UIEquipmentList list)
        {
            var toTop = _focusingList == list;
            if (list.Focus(toTop)) _focusingList = list;
        }

        private bool _hasFocusOnFirstInitializedList = false;
        private UIEquipmentList _focusingList;

        private void FocusOnFirstInitializedList(UIEquipmentList list)
        {
            if (_hasFocusOnFirstInitializedList) return;
            _hasFocusOnFirstInitializedList = true;
            FocusOnEquipmentList(list);
        }

        private void TransferEquipmentToOtherListAndFocus(UIEquipment equipment)
        {
            var otherList = _equipmentLists[0] == _focusingList ? _equipmentLists[1] : _equipmentLists[0];
            otherList.Transfer(equipment);
            _focusingList = otherList;
        }
    }
}