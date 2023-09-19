using UnityEngine;
using CryptoQuest.UI.Battle.StateMachine;

namespace CryptoQuest.UI.Battle.SelectCommand
{
    public class SelectCommandState : BattleMenuStateBase
    {
        public SelectCommandState(BattleMenuStateMachine stateMachine)
            : base(stateMachine) { }

        public override void OnEnter()
        {
            Debug.Log($"SelectCommandState::OnEnter");
            base.OnEnter();
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}