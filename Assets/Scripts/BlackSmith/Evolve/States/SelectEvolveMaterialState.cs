using CryptoQuest.BlackSmith.Evolve.UI;

namespace CryptoQuest.BlackSmith.Evolve.States
{
    public class SelectEvolveMaterialState : EvolveStateBase
    {
        private EvolveableEquipmentsPresenter _evolveableEquipmentsPresenter;

        public SelectEvolveMaterialState(EvolveStateMachine stateMachine) : base(stateMachine)
        {
            _evolveStateMachine = stateMachine;
            _evolveStateMachine.Presenter.TryGetComponent(out _evolveableEquipmentsPresenter, true);
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _evolveableEquipmentsPresenter.ClearMaterialEquipmentsIfExist();
            _evolveableEquipmentsPresenter.OnSubmitMaterialEquipment += HandleMaterialEquipmentSubmitted;
        }

        public override void OnExit()
        {
            base.OnExit();
            _evolveableEquipmentsPresenter.OnSubmitMaterialEquipment -= HandleMaterialEquipmentSubmitted;
        }

        protected override void OnCancel()
        {
            base.OnCancel();
            _evolveStateMachine.RequestStateChange(EvolveConstants.SELECT_EQUIPMENT_STATE);
        }

        private void HandleMaterialEquipmentSubmitted()
        {
            if (_evolveableEquipmentsPresenter.MaterialEquipments.Count >= _evolveableEquipmentsPresenter.MaterialRequiredCount)
                _evolveStateMachine.RequestStateChange(EvolveConstants.CONFIRM_EVOLVE_STATE);
        }
    }
}
