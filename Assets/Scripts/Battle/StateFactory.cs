using CryptoQuest.Battle.States;

namespace CryptoQuest.Battle
{
    public class StateFactory
    {
        private readonly StateMachine _fsm;

        public StateFactory(StateMachine stateMachine)
        {
            _fsm = stateMachine;
        }

        public IState CreateIntro() => new Intro(_fsm.IntroUI);

        public IState CreateCommand() => new SelectCommand(_fsm.CommandUI);
    }
}