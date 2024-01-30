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
        private RanchStateController _controller;

        private TinyMessageSubscriptionToken _getDataInGameSucceed;
        private TinyMessageSubscriptionToken _getDataInBoxSucceed;
        private TinyMessageSubscriptionToken _getDataSucceed;

        private static readonly int OverViewState = Animator.StringToHash("OverviewState");
        private static readonly int ConfirmState = Animator.StringToHash("ConfirmState");
        private bool _hasFocus;
        private bool _isOpennedDetail;

        protected override void OnEnter()
        {
            _controller = StateMachine.GetComponent<RanchStateController>();
            var uiBeastSwap = _controller.UIBeastSwap;

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

            _controller.Controller.Input.NavigateEvent += SwitchToOtherListRequested;
            _controller.Controller.Input.ExecuteEvent += SendItemsRequested;
            _controller.Controller.Input.ResetEvent += ResetTransferRequested;
            _controller.Controller.Input.CancelEvent += CancelBeastSwapState;
            _controller.Controller.Input.InteractEvent += ToggleBeastDetailVisibility;

            _controller.Controller.ShowWalletEventChannel.EnableAll().Show();
            ActionDispatcher.Dispatch(new FetchProfileBeastsAction());
        }

        protected override void OnExit()
        {
            UIBeastItem.Pressed -= UIBeastItemOnPressed;

            _controller.Controller.Input.NavigateEvent -= SwitchToOtherListRequested;
            _controller.Controller.Input.ExecuteEvent -= SendItemsRequested;
            _controller.Controller.Input.ResetEvent -= ResetTransferRequested;
            _controller.Controller.Input.CancelEvent -= CancelBeastSwapState;
            _controller.Controller.Input.InteractEvent -= ToggleBeastDetailVisibility;

            _controller.Controller.ShowWalletEventChannel.Hide();

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

        private void UIBeastItemOnPressed(UIBeastItem item) => _controller.UIBeastSwap.TransferBeast();

        private void ResetTransferRequested()
        {
            HideBeastTooltip();
            _controller.UIBeastSwap.ResetSelected();
        }

        private void SendItemsRequested()
        {
            HideBeastTooltip();
            if (!_controller.UIBeastSwap.IsValid()) return;
            StateMachine.Play(ConfirmState);
        }

        private void CancelBeastSwapState()
        {
            HideBeastTooltip();
            _controller.UIBeastSwap.Contents.SetActive(false);
            _controller.Controller.Initialize();
            _controller.UIBeastSwap.OnTransferring();
            StateMachine.Play(OverViewState);
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
            _controller.UIBeastSwap.SwitchList(direction);
            HideBeastTooltip();
        }
    }
}