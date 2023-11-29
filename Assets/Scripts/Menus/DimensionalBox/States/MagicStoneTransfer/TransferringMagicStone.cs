using CryptoQuest.Core;
using CryptoQuest.Menus.DimensionalBox.UI.MagicStoneTransfer;
using CryptoQuest.Sagas.Objects;
using TinyMessenger;
using UnityEngine;

namespace CryptoQuest.Menus.DimensionalBox.States.MagicStoneTransfer
{
    public class TransferringMagicStone : StateBase
    {
        private readonly GameObject _magicStoneTransferPanel;
        private UIMagicStoneList[] _magicStoneLists;

        private bool _hasFocusOnFirstInitializedList = false;
        private UIMagicStoneList _focusingList;

        public TransferringMagicStone(GameObject magicStoneTransferPanel)
        {
            _magicStoneTransferPanel = magicStoneTransferPanel;
            _magicStoneLists = _magicStoneTransferPanel.GetComponentsInChildren<UIMagicStoneList>();
        }

        private TinyMessageSubscriptionToken _getDataSucceed;
        private TinyMessageSubscriptionToken _getDataFailed;

        protected override void OnEnter()
        {
            _magicStoneTransferPanel.SetActive(true);
            _hasFocusOnFirstInitializedList = false;

            _getDataSucceed = ActionDispatcher.Bind<GetNftMagicStoneSucceed>(RegisterEscapeKey);
            _getDataFailed = ActionDispatcher.Bind<GetNftMagicStoneFailed>(RegisterEscapeKey);

            StateMachine.Input.MenuExecuteEvent += ConfirmTransfer;
            StateMachine.Input.MenuNavigateEvent += MoveBetweenList;
            StateMachine.Input.MenuResetEvent += ResetToOriginals;

            foreach (var magicStoneList in _magicStoneLists)
            {
                magicStoneList.Initialized += FocusOnFirstInitializedList;
                magicStoneList.Transferring += TransferMagicStoneToOtherListAndFocus;
                if (_hasFocusOnFirstInitializedList || !magicStoneList.Focus()) continue;
                _hasFocusOnFirstInitializedList = true;
            }

            ActionDispatcher.Dispatch(new GetNftMagicStone());
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

            foreach (var magicStoneList in _magicStoneLists)
            {
                magicStoneList.Initialized -= FocusOnFirstInitializedList;
                magicStoneList.Transferring -= TransferMagicStoneToOtherListAndFocus;
            }
        }

        private void ConfirmTransfer()
        {
            if (_magicStoneLists[0].PendingTransfer == false && _magicStoneLists[1].PendingTransfer == false) return;
            StateMachine.ChangeState(StateMachine.ConfirmMagicStoneTransfer);
        }

        private void MoveBetweenList(Vector2 dir)
        {
            if (_hasFocusOnFirstInitializedList == false) return;
            switch (dir.x)
            {
                case < 0:
                    FocusOnMagicStoneList(_magicStoneLists[0]);
                    break;
                case > 0:
                    FocusOnMagicStoneList(_magicStoneLists[1]);
                    break;
            }

            _focusingList.Navigate(dir.y * -1);
        }

        private void TransferMagicStoneToOtherListAndFocus(UIMagicStone ui)
        {
            var otherList = _magicStoneLists[0] == _focusingList ? _magicStoneLists[1] : _magicStoneLists[0];
            otherList.Transfer(ui);
            _focusingList = otherList;
        }

        private void FocusOnFirstInitializedList(UIMagicStoneList list)
        {
            if (_hasFocusOnFirstInitializedList) return;
            if (FocusOnMagicStoneList(list))
                _hasFocusOnFirstInitializedList = true;
        }

        private bool FocusOnMagicStoneList(UIMagicStoneList list)
        {
            var toTop = _focusingList == list;
            if (!list.Focus(toTop)) return false;
            _focusingList = list;
            return true;
        }

        private void RegisterEscapeKey(ActionBase _)
        {
            StateMachine.Input.MenuCancelEvent += ToSelectTransferTypeState;
            UpdateMagicStoneSelectingState();
        }

        private void ToSelectTransferTypeState()
        {
            _magicStoneTransferPanel.SetActive(false);
            StateMachine.ChangeState(StateMachine.Landing);
        }

        private void ResetToOriginals()
        {
            _hasFocusOnFirstInitializedList = false;
            foreach (var magicStoneList in _magicStoneLists) magicStoneList.Reset();
        }

        private void UpdateMagicStoneSelectingState()
        {
            var inGameStones = _magicStoneLists[0].ScrollView.content.GetComponentsInChildren<UIMagicStone>();

            foreach (var stoneUI in inGameStones) stoneUI.EquippedTag.SetActive(IsEquiping(stoneUI.MagicStone));

            inGameStones = _magicStoneLists[1].ScrollView.content.GetComponentsInChildren<UIMagicStone>();
            foreach (var stoneUI in inGameStones)
            {
                stoneUI.EquippedTag.SetActive(IsEquiping(stoneUI.MagicStone));
                if (!stoneUI.EquippedTag.activeSelf) continue;
                _magicStoneLists[1].OnTransferring(stoneUI);
            }
        }

        private bool IsEquiping(MagicStone magicStoneUIMagicStone)
        {
            // TODO: Implement beast inventory


            return false;
        }
    }
}