using FSM;
using CryptoQuest.PushdownFSM;
using UnityEngine;
using System;

namespace CryptoQuest.UI.Battle.MenuStateMachine
{
    public class BattleMenuStateBase : PushdownStateBase
    {
        protected BattleMenuStateMachine _battleMenuFSM;

        
        public BattleMenuStateBase(BattleMenuStateMachine stateMachine) : base(stateMachine) 
        {
            _battleMenuFSM = stateMachine;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _battleMenuFSM.ActiveState = this;
            Debug.Log($"BattleMenuStateBase:: {GetType().Name}/OnEnter");
        }

        public override void OnExit()
        {
            base.OnExit();
            Debug.Log($"BattleMenuStateBase:: {GetType().Name}/OnExit");
        }

        public override void OnLogic()
        {
            base.OnLogic();
            Debug.Log($"BattleMenuStateBase:: {GetType().Name}/OnLogic");
        }
    }
}