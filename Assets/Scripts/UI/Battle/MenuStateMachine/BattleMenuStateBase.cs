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
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void OnLogic()
        {
            base.OnLogic();
        }
    }
}