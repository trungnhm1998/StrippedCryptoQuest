using System.Collections.Generic;
using CryptoQuest.Core;
using CryptoQuest.Ranch.UI;
using CryptoQuest.Sagas.Objects;
using TinyMessenger;
using UnityEngine;

namespace CryptoQuest.Ranch.State.BeastSwap
{
    public class SwapStateBehaviour : BaseStateBehaviour
    {
        private RanchStateController _controller;

        private TinyMessageSubscriptionToken _getDataInGameSucceed;
        private TinyMessageSubscriptionToken _getDataInBoxSucceed;
        private TinyMessageSubscriptionToken _getDataSucceed;

        private List<Beast> _cachedGameData = new();
        private List<Beast> _cachedWalletData = new();

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

            ActionDispatcher.Dispatch(new GetBeasts());
        }

        protected override void OnExit()
        {
            UIBeastItem.Pressed -= UIBeastItemOnPressed;

            _controller.Controller.Input.NavigateEvent -= SwitchToOtherListRequested;
            _controller.Controller.Input.ExecuteEvent -= SendItemsRequested;
            _controller.Controller.Input.ResetEvent -= ResetTransferRequested;
            _controller.Controller.Input.CancelEvent -= CancelBeastSwapState;

            ActionDispatcher.Unbind(_getDataInGameSucceed);
            ActionDispatcher.Unbind(_getDataInBoxSucceed);
            ActionDispatcher.Unbind(_getDataSucceed);
        }

        private void ResetTransferRequested()
        {
            _controller.UIBeastSwap.InGameBeastList.SetData(_cachedGameData);
            _controller.UIBeastSwap.WalletBeastList.SetData(_cachedWalletData);

            _controller.UIBeastSwap.Focus();
        }

        private void SendItemsRequested()
        {
            StateMachine.Play(ConfirmState);
        }

        private void CancelBeastSwapState()
        {
            _controller.UIBeastSwap.Contents.SetActive(false);
            _controller.Controller.Initialize();
            StateMachine.Play(OverViewState);
        }

        private void InitializeUI(GetNftBeastsSucceed _) => _controller.UIBeastSwap.Focus();

        private void UIBeastItemOnPressed(UIBeastItem item) => _controller.UIBeastSwap.Transfer(item);

        private void GetWalletBeasts(GetInBoxBeastsSucceed beast)
        {
            _cachedWalletData = beast.WalletBeasts;
            _controller.UIBeastSwap.WalletBeastList.SetData(_cachedWalletData);
        }

        private void GetInGameBeasts(GetInGameBeastsSucceed beast)
        {
            _cachedGameData = beast.InGameBeasts;
            _controller.UIBeastSwap.InGameBeastList.SetData(_cachedGameData);
        }

        private void SwitchToOtherListRequested(Vector2 direction)
        {
            _controller.UIBeastSwap.SwitchList(direction);
        }
    }
}