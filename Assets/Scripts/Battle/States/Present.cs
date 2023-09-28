using System.Collections.Generic;

namespace CryptoQuest.Battle.States
{
    public class Present : IState
    {
        public Present(Stack<SelectHeroesActions.SelectHeroesActions.HeroCommand> heroCommands)
        {
        }

        public void OnEnter(BattleStateMachine stateMachine)
        {
            var presenter = stateMachine.GetComponent<BattlePresenter>();
            presenter.CommandPanel.SetActive(false);
        }

        public void OnExit(BattleStateMachine stateMachine)
        {
        }
    }
}