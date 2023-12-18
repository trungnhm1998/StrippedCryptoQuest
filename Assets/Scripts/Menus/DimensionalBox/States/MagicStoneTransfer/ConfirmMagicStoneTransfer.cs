using System.Collections.Generic;
using CryptoQuest.Menus.DimensionalBox.UI.MagicStoneTransfer;
using CryptoQuest.Sagas.MagicStone;
using CryptoQuest.UI.Dialogs.ChoiceDialog;
using FSM;
using IndiGames.Core.Events;
using TinyMessenger;
using UnityEngine.Localization;

namespace CryptoQuest.Menus.DimensionalBox.States.MagicStoneTransfer
{
    public class TransferringMagicStones : ActionBase
    {
        public List<UIMagicStone> ToWallet { get; set; }
        public List<UIMagicStone> ToGame { get; set; }
    }

    public class ConfirmMagicStoneTransfer : ActionState<EMagicStoneState, EStateAction>
    {
        private UIChoiceDialog _choiceDialog;
        private TransferringMagicStoneStateMachine _fsm;
        private TinyMessageSubscriptionToken _transferredEvent;
        private TinyMessageSubscriptionToken _transferFailedEvent;

        public ConfirmMagicStoneTransfer(TransferringMagicStoneStateMachine fsm) : base(false)
        {
            _fsm = fsm;
            AddAction(EStateAction.OnCancel, BackToSelect);
        }

        public override void OnEnter()
        {
            base.OnEnter();
            ChoiceDialogController.Instance.InstantiateAsync(ShowConfirmDialog);
            _fsm.DBoxList.Interactable = _fsm.IngameList.Interactable = false;
        }

        public override void OnExit()
        {
            if (_transferredEvent != null) ActionDispatcher.Unbind(_transferredEvent);
            if (_transferFailedEvent != null) ActionDispatcher.Unbind(_transferFailedEvent);
            ChoiceDialogController.Instance.Release(_choiceDialog);
        }

        private void ShowConfirmDialog(UIChoiceDialog dialog)
        {
            _choiceDialog = dialog;
            _choiceDialog
                .WithNoCallback(BackToSelect)
                .WithYesCallback(OnYesButton)
                .SelectYes()
                .SetMessage(new LocalizedString(Constants.DIMENSIONAL_BOX_TABLE_REFERENCE, Constants.CONFIRM_MESSAGE))
                .Show();
        }

        private void OnYesButton()
        {
            _transferredEvent = ActionDispatcher.Bind<TransferMagicStoneSucceed>(_ => BackToSelect());
            _transferFailedEvent = ActionDispatcher.Bind<TransferMagicStoneFailed>(_ => BackToSelect());
            ActionDispatcher.Dispatch(new TransferringMagicStones()
            {
                ToGame = _fsm.ToGame,
                ToWallet = _fsm.ToWallet
            });
            BackToSelect();
        }

        private void BackToSelect() => fsm.RequestStateChange(EMagicStoneState.SelectStone);
    }
}