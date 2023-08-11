using CryptoQuest.PushdownFSM;
using CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit;
using UnityEngine;

namespace CryptoQuest.UI.Battle.MenuStateMachine
{
    public class BattleMenuStateBase : PushdownStateBase
    {
        protected BattleMenuStateMachine _battleMenuFSM;
        protected BattlePanelController _battlePanelController;
        protected IBattleUnit _currentUnit;
        
        public BattleMenuStateBase(BattleMenuStateMachine stateMachine) : base(stateMachine) 
        {
            _battleMenuFSM = stateMachine;
            _battlePanelController = stateMachine.BattlePanelController;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _currentUnit = _battlePanelController.BattleManager.CurrentUnit;
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