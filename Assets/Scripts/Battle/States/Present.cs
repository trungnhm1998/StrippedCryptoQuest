using CryptoQuest.Battle.Events;
using TinyMessenger;

namespace CryptoQuest.Battle.States
{
    public class Present : IState
    {
        private BattlePresenter _presenter;
        private BattleContext _battleContext;
        private BattleStateMachine _stateMachine;
        private TinyMessageSubscriptionToken _roundEndedEventToken;

        public void OnEnter(BattleStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            _battleContext = stateMachine.GetComponent<BattleContext>();
            var presentation = stateMachine.GetComponent<PresentBehaviour>();
            _presenter = stateMachine.GetComponent<BattlePresenter>();
            _presenter.CommandPanel.SetActive(false);

            var sortedAliveCharacterBasedOnAgi = _battleContext.GetSortedAliveCharacterBasedOnAgi();
            presentation.ExecuteCharacterCommands(sortedAliveCharacterBasedOnAgi);
            _roundEndedEventToken = BattleEventBus.SubscribeEvent<RoundEndedEvent>(BackToSelectHeroesAction);
        }

        public void OnExit(BattleStateMachine stateMachine)
        {
            BattleEventBus.UnsubscribeEvent(_roundEndedEventToken);
        }

        private void BackToSelectHeroesAction(RoundEndedEvent eventObject)
        {
            _stateMachine.ChangeState(new SelectHeroesActions.SelectHeroesActions());
        }
    }
}