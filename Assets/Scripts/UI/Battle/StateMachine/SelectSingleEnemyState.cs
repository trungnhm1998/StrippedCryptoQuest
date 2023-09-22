using System;
using CryptoQuest.UI.Battle.StateMachine;

namespace CryptoQuest.UI.Battle.StateMachine
{
    public class SelectSingleEnemyState : BattleMenuStateBase
    {
        public static event Action EnteredState;

        public SelectSingleEnemyState(BattleMenuStateMachine stateMachine)
            : base(stateMachine) { }

        public override void OnEnter()
        {
            base.OnEnter();
            EnteredState?.Invoke();
        }
    }
}