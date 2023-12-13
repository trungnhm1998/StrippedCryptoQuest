namespace CryptoQuest.BlackSmith.Evolve.States
{
    public class EvolveResult : EvolveStateBase
    {
        public EvolveResult(EvolveStateMachine stateMachine) : base(stateMachine) { }

        public override void OnEnter()
        {
            base.OnEnter();
            EquipmentsPresenter.gameObject.SetActive(false);
            EvolveSystem.EquipmentDetailPresenter.gameObject.SetActive(false);
            EvolveResultPresenter.gameObject.SetActive(true);
        }

        public override void OnExit()
        {
            base.OnExit();
            EvolveResultPresenter.gameObject.SetActive(false);
        }

        public override void OnCancel()
        {
            base.OnCancel();
            fsm.RequestStateChange(EStates.SelectEquipment);
        }

        public override void OnSubmit()
        {
            base.OnCancel();
            fsm.RequestStateChange(EStates.SelectEquipment);
        }
    }
}
