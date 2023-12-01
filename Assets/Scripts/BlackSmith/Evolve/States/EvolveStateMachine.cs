using CryptoQuest.BlackSmith.Evolve.UI;
using FSM;

namespace CryptoQuest.BlackSmith.Evolve.States
{
    public static class EvolveConstants
    {
        public const string SELECT_EQUIPMENT_STATE = "SELECT_EQUIPMENT_STATE";
        public const string SELECT_MATERIAL_STATE = "SELECT_MATERIAL_STATE";
        public const string CONFIRM_EVOLVE_STATE = "CONFIRM_EVOLVE_STATE";
        public const string EVOLVE_SUCCESS_STATE = "EVOLVE_SUCCESS_STATE";
        public const string EVOLVE_FAIL_STATE = "EVOLVE_FAIL_STATE";
    }

    public class EvolveStateMachine : StateMachine
    {
        public readonly EvolvePresenter Presenter;
        public readonly BlackSmithStateMachine RootStateMachine;

        public EvolveStateMachine(BlackSmithStateMachine stateMachine) : base(false)
        {
            RootStateMachine = stateMachine;
            Presenter = RootStateMachine.BlackSmithManager.EvolvePresenter;

            AddState(EvolveConstants.SELECT_EQUIPMENT_STATE, new SelectEvolveEquipmentState(this));
            AddState(EvolveConstants.SELECT_MATERIAL_STATE, new SelectEvolveMaterialState(this));
            AddState(EvolveConstants.CONFIRM_EVOLVE_STATE, new ConfirmEvolveState(this));
            AddState(EvolveConstants.EVOLVE_SUCCESS_STATE, new EvolveSuccessState(this));
            AddState(EvolveConstants.EVOLVE_FAIL_STATE, new EvolveFailState(this));

            SetStartState(EvolveConstants.SELECT_EQUIPMENT_STATE);
        }

        public override void OnEnter()
        {
            base.OnEnter();
            Presenter.gameObject.SetActive(true);
            RequestStateChange(EvolveConstants.SELECT_EQUIPMENT_STATE);
        }

        public override void OnExit() { }
    }
}
