using CryptoQuest.Ranch.UI;
using CryptoQuest.Sagas.Objects;
using CryptoQuest.UI.Tooltips.Events;
using IndiGames.Core.Events;
using TinyMessenger;
using UnityEngine;

namespace CryptoQuest.Ranch.State.BeastSwap
{
    public class SwapStateBehaviour : BaseStateBehaviour
    {
        [SerializeField] private ShowTooltipEvent _showTooltipEventChannelSO;

        private TinyMessageSubscriptionToken _getDataInGameSucceed;
        private TinyMessageSubscriptionToken _getDataInBoxSucceed;
        private TinyMessageSubscriptionToken _getDataSucceed;

        private bool _hasFocus;
        private bool _isOpennedDetail;

        protected override void OnEnter()
        {
            var uiBeastSwap = _stateController.UIBeastSwap;

            uiBeastSwap.Contents.SetActive(true);
            uiBeastSwap.InBoxBeastList.Clear();
            uiBeastSwap.InGameBeastList.Clear();
            _hasFocus = false;
            _isOpennedDetail = false;

            _getDataSucceed = ActionDispatcher.Bind<GetBeastSucceeded>(_ => _hasFocus = false);

            _getDataInGameSucceed =
                ActionDispatcher.Bind<FetchInGameBeastSucceeded>(ctx =>
                    FillBeasts(uiBeastSwap.InGameBeastList, ctx.InGameBeasts));
            _getDataInBoxSucceed =
                ActionDispatcher.Bind<FetchInboxBeastSucceeded>(data =>
                    FillBeasts(uiBeastSwap.InBoxBeastList, data.InBoxBeasts));

            UIBeastItem.Pressed += UIBeastItemOnPressed;

            _input.NavigateEvent += SwitchToOtherListRequested;
            _input.ExecuteEvent += SendItemsRequested;
            _input.ResetEvent += ResetTransferRequested;
            _input.CancelEvent += CancelBeastSwapState;
            _input.ShowDetailEvent += ToggleBeastDetailVisibility;

            _stateController.Controller.ShowWalletEventChannel.EnableAll().Show();
            ActionDispatcher.Dispatch(new FetchProfileBeastsAction());
        }

        protected override void OnExit()
        {
            UIBeastItem.Pressed -= UIBeastItemOnPressed;

            _input.NavigateEvent -= SwitchToOtherListRequested;
            _input.ExecuteEvent -= SendItemsRequested;
            _input.ResetEvent -= ResetTransferRequested;
            _input.CancelEvent -= CancelBeastSwapState;
            _input.ShowDetailEvent -= ToggleBeastDetailVisibility;

            _stateController.Controller.ShowWalletEventChannel.Hide();

            ActionDispatcher.Unbind(_getDataInGameSucceed);
            ActionDispatcher.Unbind(_getDataInBoxSucceed);
            ActionDispatcher.Unbind(_getDataSucceed);
        }

        private void ToggleBeastDetailVisibility()
        {
            _isOpennedDetail = !_isOpennedDetail;
            _showTooltipEventChannelSO.RaiseEvent(_isOpennedDetail);
        }

        private void HideBeastTooltip()
        {
            _isOpennedDetail = false;
            _showTooltipEventChannelSO.RaiseEvent(_isOpennedDetail);
        }

        private void UIBeastItemOnPressed(UIBeastItem item) => _stateController.UIBeastSwap.TransferBeast();

        private void ResetTransferRequested()
        {
            HideBeastTooltip();
            _stateController.UIBeastSwap.ResetSelected();
        }

        private void SendItemsRequested()
        {
            HideBeastTooltip();
            if (!_stateController.UIBeastSwap.IsValid()) return;
            StateMachine.Play(SwapConfirmState);
        }

        private void CancelBeastSwapState()
        {
            HideBeastTooltip();
            _stateController.UIBeastSwap.Contents.SetActive(false);
            _stateController.Controller.Initialize();
            _stateController.UIBeastSwap.OnTransferring();
            StateMachine.Play(OverviewState);
        }

        private void FillBeasts(UIBeastList uiList, BeastResponse[] beasts)
        {
            uiList.Initialize(beasts);
            uiList.Interactable = false;

            if (_hasFocus)
            {
                _hasFocus = false;
                return;
            }

            _hasFocus = uiList.TryFocus();
        }

        private void SwitchToOtherListRequested(Vector2 direction)
        {
            _stateController.UIBeastSwap.SwitchList(direction);
            HideBeastTooltip();
        }
    }
}