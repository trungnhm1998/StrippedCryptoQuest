using CryptoQuest.BlackSmith.Evolve.UI;
using FSM;

namespace CryptoQuest.BlackSmith.Evolve.States
{
    public enum EStates
    {
        SelectEquipment,
        SelectMaterial,
        ConfirmEvolve,
        EvolveSuccess,
        EvolveFailed
    }

    public class EvolveStateMachine : StateMachine<string, EStates, string>
    {
        private readonly BlackSmithSystem _context;
        private EvolveStateBase _state;
        public EvolveSystem EvolveSystem { get; }
        public UIEquipmentItem ItemToEvolve { get; set; }
        public UIEquipmentItem MaterialItem { get; set; }

        public EvolveStateMachine(BlackSmithSystem context)
        {
            _context = context;
            EvolveSystem = _context.EvolveSystem;

            AddState(EStates.SelectEquipment, new SelectEquipmentToEvolve(this));
            AddState(EStates.SelectMaterial, new SelectEvolveMaterial(this));
            AddState(EStates.ConfirmEvolve, new ConfirmEvolve(this));
            AddState(EStates.EvolveSuccess, new EvolveSuccess(this));
            AddState(EStates.EvolveFailed, new EvolveFailed(this));

            SetStartState(EStates.SelectEquipment);
        }

        public void SetCurrentState(EvolveStateBase state) => _state = state;

        public override void OnEnter()
        {
            EvolveSystem.gameObject.SetActive(true);
            _context.Input.CancelEvent += OnCancel;
            _context.Input.SubmitEvent += OnSubmit;
            base.OnEnter();
        }

        public override void OnExit()
        {
            EvolveSystem.gameObject.SetActive(false);
            _context.Input.CancelEvent -= OnCancel;
            _context.Input.SubmitEvent -= OnSubmit;
            base.OnExit();
        }

        private void OnCancel() => _state?.OnCancel();

        private void OnSubmit() => _state?.OnSubmit();

        public void BackToOverview() => fsm.RequestStateChange(State.OVERVIEW);
    }
}