using IndiGames.Core.Events;
using TinyMessenger;

namespace CryptoQuest.Menus.DimensionalBox.States.MetadTransfer
{
    public class FetchToken : TransferMetadBaseState
    {
        private TinyMessageSubscriptionToken _getTokenSucceed;
        private TinyMessageSubscriptionToken _getTokenFailed;
    
        public FetchToken(TransferMetadStateMachine fsm) : base(fsm) { }
    
        public override void OnEnter()
        {
            _panel.SetInteractable(false);
            ActionDispatcher.Dispatch(new GetToken());
            _getTokenSucceed = ActionDispatcher.Bind<GetTokenSuccess>(
                _ => _fsm.RequestStateChange(EMetadState.SelectSource));
            _getTokenFailed = ActionDispatcher.Bind<GetTokenFailed>(_ => _fsm.BackToOverview());
        }

        public override void OnExit()
        {
            _panel.SetInteractable(true);
            ActionDispatcher.Unbind(_getTokenSucceed);
            ActionDispatcher.Unbind(_getTokenFailed);
        }
    }
}