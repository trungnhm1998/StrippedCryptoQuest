using CryptoQuest.BlackSmith.Upgrade.Presenters;
using CryptoQuest.BlackSmith.Upgrade.UI;
using CryptoQuest.Input;
using FSM;
using UnityEngine;

namespace CryptoQuest.BlackSmith.Upgrade.States
{
    public abstract class UpgradeStateBase : StateBase<EStates>
    {
        protected UpgradeStateMachine StateMachine { get; }
        protected UpgradeSystem UpgradeSystem => StateMachine.UpgradeSystem;
        protected EquipmentsPresenter EquipmentsPresenter => UpgradeSystem.EquipmentsPresenter;
        protected EquipmentDetailsPresenter EquipmentDetailsPresenter => UpgradeSystem.EquipmentDetailsPresenter;
        protected ConfigUpgradePresenter ConfigUpgradePresenter => UpgradeSystem.ConfigUpgradePresenter;
        protected UIConfirmDetails UIConfirmDetails => UpgradeSystem.UIConfirmDetails;
        protected GameObject ResultUI => UpgradeSystem.ResultUI;
        protected CurrencyPresenter CurrencyPresenter => UpgradeSystem.CurrencyPresenter;
        protected MerchantsInputManager Input => StateMachine.Input;
        protected BlackSmithDialogsPresenter DialogsPresenter => StateMachine.DialogsPresenter;

        protected UpgradeStateBase(UpgradeStateMachine stateMachine) : base(false)
            => StateMachine = stateMachine;

        public override void OnEnter() => StateMachine.SetCurrentState(this);

        public override void OnExit() => StateMachine.SetCurrentState(null);

        public virtual void OnCancel() { }
        public virtual void OnSubmit() { }
    }
}