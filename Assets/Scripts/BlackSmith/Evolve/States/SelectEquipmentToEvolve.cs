using CryptoQuest.BlackSmith.Evolve.UI;

namespace CryptoQuest.BlackSmith.Evolve.States
{
    public class SelectEquipmentToEvolve : EvolveStateBase
    {
        public SelectEquipmentToEvolve(EvolveStateMachine stateMachine) : base(stateMachine) { }

        public override void OnEnter()
        {
            base.OnEnter();
            StateMachine.MaterialItem = null;
            StateMachine.ItemToEvolve = null;
            EquipmentsPresenter.gameObject.SetActive(true);
            EvolvableEquipmentList.EquipmentSelected += OnSelectBaseItem;

            EquipmentsPresenter.Init();
        }

        public override void OnExit()
        {
            base.OnExit();
            EvolvableEquipmentList.EquipmentSelected -= OnSelectBaseItem;
        }

        public override void OnCancel()
        {
            EquipmentsPresenter.gameObject.SetActive(false);
            StateMachine.BackToOverview();
        }

        private void OnSelectBaseItem(UIEquipmentItem item)
        {
            StateMachine.ItemToEvolve = item;
            StateMachine.RequestStateChange(EStates.SelectMaterial);
        }
    }
}