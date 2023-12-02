namespace CryptoQuest.BlackSmith.Evolve.States
{
    public class EvolveFailed : EvolveStateBase
    {
        public EvolveFailed(EvolveStateMachine stateMachine) : base(stateMachine) { }

        public override void OnEnter()
        {
            base.OnEnter();
            // _evolveResultPresenter.gameObject.SetActive(true);
            // _evolveResultPresenter.SetResultFail(null);
        }

        public override void OnExit()
        {
            base.OnExit();
            // _evolveResultPresenter.gameObject.SetActive(false);
        }

        public override void OnCancel()
        {
            base.OnCancel();
            // _evolvableEquipmentsPresenter.ReloadEquipments();
            // _evolvableEquipmentsPresenter.ClearMaterialEquipmentsIfExist();
            // _evolveStateMachine.RequestStateChange(State.SELECT_EQUIPMENT);
        }
    }
}