using System;
using CryptoQuest.BlackSmith.Evolve.States;
using CryptoQuest.BlackSmith.States.Overview;
using CryptoQuest.BlackSmith.Upgrade.States;
using FSM;

namespace CryptoQuest.BlackSmith
{
    public static class State
    {
        public const string OVERVIEW = "Overview";
        public const string UPGRADING = "Upgrading";
        public const string EVOLVING = "Evolve";
    }

    public class BlackSmithStateMachine : StateMachine<string, string, string>
    {
        public event Action Exiting;

        public BlackSmithStateMachine(BlackSmithSystem context)
        {
            AddState(State.OVERVIEW, new OverviewState(context));
            AddState(State.UPGRADING, new UpgradeStateMachine(context));
            AddState(State.EVOLVING, new EvolveStateMachine(context));

            SetStartState(State.OVERVIEW);
        }
    }
}