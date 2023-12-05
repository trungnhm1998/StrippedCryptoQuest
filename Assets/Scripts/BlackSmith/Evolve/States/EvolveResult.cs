namespace CryptoQuest.BlackSmith.Evolve.States
{
    public class EvolveResult : EvolveStateBase
    {
        public EvolveResult(EvolveStateMachine stateMachine) : base(stateMachine) { }

        public override void OnEnter()
        {
            base.OnEnter();
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
            // TODO: reload equipments
        }
    }
}
