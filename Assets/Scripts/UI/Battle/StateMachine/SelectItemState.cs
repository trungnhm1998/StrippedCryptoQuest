using System;
using CryptoQuest.UI.Battle.StateMachine;

namespace CryptoQuest.UI.Battle.StateMachine
{
    public class SelectItemState : BattleMenuStateBase
    {
        public static event Action EnteredState;

        public SelectItemState(BattleMenuStateMachine stateMachine)
            : base(stateMachine) { }

        public override void OnEnter()
        {
            base.OnEnter();
            EnteredState?.Invoke();
        }
    }
}