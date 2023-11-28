using FSM;
using UnityEngine;

namespace CryptoQuest.BlackSmith.State.Upgrade
{
    public class UpgradeState : BlackSmithStateBase
    {
        public UpgradeState(BlackSmithStateMachine stateMachine) : base(stateMachine) { }

        public override void OnEnter()
        {
            base.OnEnter();
            Debug.Log($"Entered Upgrade");
        }

        public override void OnExit()
        {
            base.OnExit();
            Debug.Log($"Exited Upgrade");
        }

        protected override void OnCancel()
        {
            fsm.RequestStateChange(Contants.OVERVIEW_STATE);
        }
    }
}