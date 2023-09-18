using FSM;
using UnityEngine;
using System;

namespace CryptoQuest.PushdownFSM
{
    public class PushdownStateBase : StateBase
    {
        protected IPushdownStateMachine<string, StateBase> _pushdownFSM;

        public PushdownStateBase(IPushdownStateMachine<string, StateBase> stateMachine) : base(false) 
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