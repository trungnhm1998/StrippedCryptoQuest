using System.Collections.Generic;
using CryptoQuest.UI.Dialogs.ChoiceDialog;
using FSM;
using IndiGames.Core.Events;
using TinyMessenger;
using UnityEngine.Localization;

namespace CryptoQuest.Menus.DimensionalBox.States.EquipmentsTransfer
{
    public class Confirm : ActionState<EEquipmentState, EStateAction>
    {
        private UIChoiceDialog _dialog;
        private TransferEquipmentsStateMachine _fsm;
        private TinyMessageSubscriptionToken _transferredEvent;
        private TinyMessageSubscriptionToken _transferFailedEvent;

        public Confirm(TransferEquipmentsStateMachine fsm) : base(false)
        {
            _fsm = fsm;
            AddAction(EStateAction.OnCancel, BackToSelect);
        }

        public override void OnEnter()
        {
            base.OnEnter();
            ChoiceDialogController.Instance.Instantiate(ShowConfirmDialog);
            _fsm.InboxList.Interactable = _fsm.IngameList.Interactable = false;
        }

        private void ShowConfirmDialog(UIChoiceDialog dialog)
        {
            _dialog = dialog;
            dialog
                .WithYesCallback(() =>
                {
                    _transferredEvent = ActionDispatcher.Bind<TransferSucceed>(_ => BackToSelect());
                    _transferFailedEvent =
                        ActionDispatcher.Bind<TransferFailed>(_ => BackToSelect());
                    ActionDispatcher.Dispatch(new TransferringEquipments()
                    {
                        ToGame = _fsm.ToGame,
                        ToWallet = _fsm.ToWallet
                    });
                })
                .WithNoCallback(BackToSelect)
                .SelectYes()
                .SetMessage(new LocalizedString(Constants.DIMENSIONAL_BOX_TABLE_REFERENCE, Constants.CONFIRM_MESSAGE))
                .Show();
        }

        private void BackToSelect() => fsm.RequestStateChange(EEquipmentState.SelectEquipment);

        public override void OnExit()
        {
            if (_transferredEvent != null) ActionDispatcher.Unbind(_transferredEvent);
            if (_transferFailedEvent != null) ActionDispatcher.Unbind(_transferFailedEvent);
            ChoiceDialogController.Instance.Release(_dialog);
        }
    }

    public class TransferringEquipments : ActionBase
    {
        public List<uint> ToWallet { get; set; }
        public List<uint> ToGame { get; set; }
    }
}