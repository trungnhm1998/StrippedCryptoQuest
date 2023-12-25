using System.Collections.Generic;
using CryptoQuest.Beast;
using CryptoQuest.Ranch.Sagas;
using CryptoQuest.Ranch.UI;
using CryptoQuest.UI.Tooltips.Events;
using IndiGames.Core.Common;
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

        private List<IBeast> _cachedGameData = new();
        private List<IBeast> _cachedWalletData = new();

        private static readonly int OverViewState = Animator.StringToHash("OverviewState");
        private static readonly int ConfirmState = Animator.StringToHash("ConfirmState");

        protected override void OnEnter()
        {
            _controller = StateMachine.GetComponent<RanchStateController>();

            _getDataSucceed = ActionDispatcher.Bind<GetNftBeastsSucceed>(InitializeUI);
            _getDataInGameSucceed = ActionDispatcher.Bind<GetInGameBeastsSucceed>(GetInGameBeasts);
            _getDataInBoxSucceed = ActionDispatcher.Bind<GetInBoxBeastsSucceed>(GetWalletBeasts);

            _controller.UIBeastSwap.Contents.SetActive(true);
            _controller.UIBeastSwap.SelectedWalletBeatIds.Clear();
            _controller.UIBeastSwap.SelectedInGameBeatIds.Clear();

            UIBeastItem.Pressed += UIBeastItemOnPressed;

            _controller.Controller.Input.NavigateEvent += SwitchToOtherListRequested;
            _controller.Controller.Input.ExecuteEvent += SendItemsRequested;
            _controller.Controller.Input.ResetEvent += ResetTransferRequested;
            _controller.Controller.Input.CancelEvent += CancelBeastSwapState;
            _controller.Controller.Input.ShowDetailEvent += ShowBeastDetail;

            _controller.Controller.ShowWalletEventChannel.EnableAll().Show();
            ResetBeastData();
            ActionDispatcher.Dispatch(new GetBeasts());
        }

        protected override void OnExit()
        {
            UIBeastItem.Pressed -= UIBeastItemOnPressed;

            _controller.Controller.Input.NavigateEvent -= SwitchToOtherListRequested;
            _controller.Controller.Input.ExecuteEvent -= SendItemsRequested;
            _controller.Controller.Input.ResetEvent -= ResetTransferRequested;
            _controller.Controller.Input.CancelEvent -= CancelBeastSwapState;
            _controller.Controller.Input.ShowDetailEvent -= ShowBeastDetail;

            _controller.Controller.ShowWalletEventChannel.Hide();

            ActionDispatcher.Unbind(_getDataInGameSucceed);
            ActionDispatcher.Unbind(_getDataInBoxSucceed);
            ActionDispatcher.Unbind(_getDataSucceed);
        }

        private void ResetBeastData()
        {
            _cachedGameData = new List<IBeast>();
            _cachedWalletData = new List<IBeast>();
        }

        private void ShowBeastDetail()
        {
            _showTooltipEventChannelSO.RaiseEvent(true);
        }

        private void HideBeastDetail()
        {
            _showTooltipEventChannelSO.RaiseEvent(false);
        }

        private void ResetTransferRequested()
        {
            HideBeastDetail();
            DisablePendingTag();
            _controller.UIBeastSwap.InGameBeastList.SetData(_cachedGameData);
            _controller.UIBeastSwap.WalletBeastList.SetData(_cachedWalletData);
            _controller.UIBeastSwap.Focus();
        }

        private void DisablePendingTag()
        {
            _controller.UIBeastSwap.InGameBeastList.UpdateList();
            _controller.UIBeastSwap.WalletBeastList.UpdateList();
        }

        private void SendItemsRequested()
        {
            HideBeastDetail();
            if (!_controller.UIBeastSwap.IsValid()) return;
            StateMachine.Play(ConfirmState);
        }

        private void CancelBeastSwapState()
        {
            HideBeastDetail();
            DisablePendingTag();
            _controller.UIBeastSwap.Contents.SetActive(false);
            _controller.Controller.Initialize();
            StateMachine.Play(OverViewState);
        }

        private void InitializeUI(GetNftBeastsSucceed _) => _controller.UIBeastSwap.Focus();

        private void UIBeastItemOnPressed(UIBeastItem item) => _controller.UIBeastSwap.OnBeastSelected(item);
        private void GetWalletBeasts(GetInBoxBeastsSucceed beast)
        {
            foreach (var data in beast.WalletBeasts)
            {
                var newBeast = ServiceProvider.GetService<IBeastResponseConverter>().Convert(data);
                _cachedWalletData.Add(newBeast);
            }

            _controller.UIBeastSwap.WalletBeastList.SetData(_cachedWalletData);
        }

        private void GetInGameBeasts(GetInGameBeastsSucceed beast)
        {
            foreach (var data in beast.InGameBeasts)
            {
                var newBeast = ServiceProvider.GetService<IBeastResponseConverter>().Convert(data);
                _cachedGameData.Add(newBeast);
            }

            _controller.UIBeastSwap.InGameBeastList.SetData(_cachedGameData);
        }

        private void SwitchToOtherListRequested(Vector2 direction)
        {
            _controller.UIBeastSwap.SwitchList(direction);
            HideBeastDetail();
        }
    }
}