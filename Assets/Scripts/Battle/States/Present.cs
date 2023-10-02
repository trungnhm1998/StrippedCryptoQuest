namespace CryptoQuest.Battle.States
{
    public class Present : IState
    {
        private BattlePresenter _presenter;
        private BattleContext _battleContext;
        private BattleStateMachine _stateMachine;

        public void OnEnter(BattleStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            _battleContext = stateMachine.GetComponent<BattleContext>();
            var presentation = stateMachine.GetComponent<Presentation>();
            _presenter = stateMachine.GetComponent<BattlePresenter>();
            _presenter.CommandPanel.SetActive(false);

            var sortedAliveCharacterBasedOnAgi = _battleContext.GetSortedAliveCharacterBasedOnAgi();
            presentation.ExecuteCharacterCommands(sortedAliveCharacterBasedOnAgi, BackToSelectHeroesAction);
        }

        public void OnExit(BattleStateMachine stateMachine) { }

        private void BackToSelectHeroesAction()
        {
            _stateMachine.ChangeState(new SelectHeroesActions.SelectHeroesActions());
        }
    }
}