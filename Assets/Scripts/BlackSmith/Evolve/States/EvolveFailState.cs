using CryptoQuest.BlackSmith.Evolve.UI;

namespace CryptoQuest.BlackSmith.Evolve.States
{
    public class EvolveFailState : EvolveStateBase
    {
        private EvolveResultPresenter _evolveResultPresenter;
        private EvolveableEquipmentsPresenter _evolveableEquipmentsPresenter;

        public EvolveFailState(EvolveStateMachine stateMachine) : base(stateMachine)
        {
            _evolveStateMachine = stateMachine;
            _evolveStateMachine.Presenter.TryGetComponent(out _evolveResultPresenter, true);
            _evolveStateMachine.Presenter.TryGetComponent(out _evolveableEquipmentsPresenter, true);
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _evolveResultPresenter.gameObject.SetActive(true);
            _evolveResultPresenter.SetResultFail(null);
        }

        public override void OnExit()
        {
            base.OnExit();
            _evolveResultPresenter.gameObject.SetActive(false);
        }

        protected override void OnCancel()
        {
            base.OnCancel();
            _evolveableEquipmentsPresenter.ReloadEquipments();
            _evolveableEquipmentsPresenter.ClearMaterialEquipmentsIfExist();
            _evolveStateMachine.RequestStateChange(EvolveConstants.SELECT_EQUIPMENT_STATE);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            RemoveEvents();
        }
    }
}
