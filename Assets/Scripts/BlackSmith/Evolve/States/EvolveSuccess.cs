namespace CryptoQuest.BlackSmith.Evolve.States
{
    public class EvolveSuccess : EvolveResult
    {
        public EvolveSuccess(EvolveStateMachine stateMachine) : base(stateMachine) { }

        public override void OnEnter()
        {
            base.OnEnter();
            EvolveResultPresenter.SetResultSuccess(null);
        }
    }
}
