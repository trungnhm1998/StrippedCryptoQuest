namespace CryptoQuest.BlackSmith.Evolve.States
{
    public class EvolveFailed : EvolveResult
    {
        public EvolveFailed(EvolveStateMachine stateMachine) : base(stateMachine) { }

        public override void OnEnter()
        {
            base.OnEnter();
            DialogsPresenter.Dialogue.SetMessage(EvolveSystem.EvolveFailText).Show();
        }
    }
}
