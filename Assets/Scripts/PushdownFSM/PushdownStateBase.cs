using FSM;
using UnityEngine;
using System;

namespace CryptoQuest.PushdownFSM
{
    public class PushdownStateBase : StateBase
    {
        protected PushdownStateMachine _pushdownFSM;

        public PushdownStateBase(PushdownStateMachine stateMachine) : base(false) 
        {
            _pushdownFSM = stateMachine;
        }

        public override void OnEnter()
        {
            _pushdownFSM.StateStack.Push(this);
            base.OnEnter();
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}