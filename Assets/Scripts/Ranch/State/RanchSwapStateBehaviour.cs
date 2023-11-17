using System.Collections.Generic;
using CryptoQuest.Actions;
using CryptoQuest.Core;
using CryptoQuest.Ranch.UI;
using TinyMessenger;
using UnityEngine;
using Obj = CryptoQuest.Sagas.Objects;

namespace CryptoQuest.Ranch.State
{
    public class RanchSwapStateBehaviour : BaseStateBehaviour
    {
        private RanchStateController _controller;

        private TinyMessageSubscriptionToken _getGameDataSucceedEvent;
        private TinyMessageSubscriptionToken _getWalletDataSucceedEvent;

        private List<Obj.Beast> _cachedGameData = new List<Obj.Beast>();
        private List<Obj.Beast> _cachedWalletData = new List<Obj.Beast>();

        private static readonly int OverView = Animator.StringToHash("OverviewState");

        protected override void OnEnter()
        {
            _controller = StateMachine.GetComponent<RanchStateController>();
            
            _controller.UIBeastOrganization.Contents.SetActive(true);
            _controller.UIBeastOrganization.SelectedWalletBeatIds.Clear();
            _controller.UIBeastOrganization.SelectedInGameBeatIds.Clear();
            _controller.UIBeastOrganization.HandleListEnable();

            UIBeastItem.Pressed += UIBeastItemOnPressed;

            _getGameDataSucceedEvent = ActionDispatcher.Bind<GetGameNftBeastsSucceed>(GetInGameBeasts);
            _getWalletDataSucceedEvent = ActionDispatcher.Bind<GetWalletNftBeastsSucceed>(GetWalletBeasts);

            _controller.RanchController.Input.CancelEvent += ExitState;

            ActionDispatcher.Dispatch(new GetBeasts());
        }

        private void UIBeastItemOnPressed(UIBeastItem item) => _controller.UIBeastOrganization.Transfer(item);

        private void GetWalletBeasts(GetWalletNftBeastsSucceed beast)
        {
            _cachedWalletData = beast.WalletBeasts;
            _controller.UIWalletList.SetData(beast.WalletBeasts);
        }

        private void GetInGameBeasts(GetGameNftBeastsSucceed beast)
        {
            _cachedGameData = beast.InGameBeasts;
            _controller.UIGameList.SetData(beast.InGameBeasts);
        }


        protected override void OnExit()
        {
            _controller.RanchController.Input.CancelEvent -= ExitState;
        }

        private void ExitState()
        {
            _controller.UIBeastOrganization.StopHandleListEnable();

            UIBeastItem.Pressed -= UIBeastItemOnPressed;

            ActionDispatcher.Unbind(_getGameDataSucceedEvent);
            ActionDispatcher.Unbind(_getWalletDataSucceedEvent);

            StateMachine.Play(OverView);
            _controller.RanchController.Initialize();
        }
    }
}