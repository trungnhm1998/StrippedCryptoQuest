using CryptoQuest.BlackSmith.Evolve.UI;
using FSM;

namespace CryptoQuest.BlackSmith.Evolve.States
{
    public abstract class EvolveStateBase : StateBase<EStates>
    {
        protected EvolveStateMachine StateMachine { get; }
        protected UIEvolvableEquipmentList EvolvableEquipmentList => StateMachine.EvolveSystem.EvolvableEquipmentListUI;
        protected EquipmentsPresenter EquipmentsPresenter => StateMachine.EvolveSystem.EquipmentsPresenter;
        protected ConfirmEvolveDialog ConfirmEvolveDialog => StateMachine.EvolveSystem.ConfirmEvolveDialog;

        protected EvolveStateBase(EvolveStateMachine stateMachine) : base(false) => StateMachine = stateMachine;

        public override void OnEnter() => StateMachine.SetCurrentState(this);

        public override void OnExit() => StateMachine.SetCurrentState(null);

        public virtual void OnCancel() { }
        public virtual void OnSubmit() { }
    }
}