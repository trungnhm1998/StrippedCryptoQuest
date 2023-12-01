using System;
using CryptoQuest.BlackSmith.Evolve.UI;

namespace CryptoQuest.BlackSmith.Evolve.States
{
    public class SelectEvolveEquipmentState : EvolveStateBase
    {
        private EvolveableEquipmentsPresenter _evolveableEquipmentsPresenter;
        private EquipmentDetailPresenter _equipmentDetailPresenter;

        public SelectEvolveEquipmentState(EvolveStateMachine stateMachine) : base(stateMachine)
        {
            _evolveStateMachine = stateMachine;
            _evolveStateMachine.Presenter.TryGetComponent(out _evolveableEquipmentsPresenter, true);
            _evolveStateMachine.Presenter.TryGetComponent(out _equipmentDetailPresenter, true);
        }

        public override void OnEnter()
        {
            base.OnEnter();
            if (_evolveableEquipmentsPresenter != null)
            {
                _evolveableEquipmentsPresenter.gameObject.SetActive(true);
                _evolveableEquipmentsPresenter.RemoveBaseEquipmentIfExist();
                SelectDefaultButton();
                _evolveableEquipmentsPresenter.OnSubmitBaseEquipment += HandleBaseEquipmentSubmitted;
                _evolveableEquipmentsPresenter.OnEquipmentRendered += SelectDefaultButton;
            }

            if (_equipmentDetailPresenter != null)
                _equipmentDetailPresenter.gameObject.SetActive(true);
        }

        protected override void OnCancel()
        {
            base.OnCancel();
            if (_evolveableEquipmentsPresenter != null)
            {
                _evolveableEquipmentsPresenter.gameObject.SetActive(false);
                _evolveableEquipmentsPresenter.OnSubmitBaseEquipment -= HandleBaseEquipmentSubmitted;
                _evolveableEquipmentsPresenter.OnEquipmentRendered -= SelectDefaultButton;
            }

            if (_equipmentDetailPresenter != null)
                _equipmentDetailPresenter.gameObject.SetActive(false);

            _evolveStateMachine.RootStateMachine.RequestStateChange(Contants.OVERVIEW_STATE);
        }

        private void SelectDefaultButton()
        {
            if (_evolveableEquipmentsPresenter.EvolveEquipmentListUI.EquipmentList.Count > 0)
                _evolveableEquipmentsPresenter.EvolveEquipmentListUI.SelectDefaultButton();
        }

        private void HandleBaseEquipmentSubmitted()
        {
            if (_evolveableEquipmentsPresenter.BaseEquipment != null && _evolveStateMachine != null)
                _evolveStateMachine.RequestStateChange(EvolveConstants.SELECT_MATERIAL_STATE);
        }
    }
}
