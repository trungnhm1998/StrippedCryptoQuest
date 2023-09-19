using CryptoQuest.PushdownFSM;

namespace CryptoQuest.UI.Battle.StateMachine
{
    public class BattleMenuStateBase : PushdownStateBase
    {
        protected BattleMenuStateMachine _battleMenuFSM;
        protected BattleMenuController _battleMenuController;
        
        public BattleMenuStateBase(BattleMenuStateMachine stateMachine) : base(stateMachine) 
        {
            _battleMenuFSM = stateMachine;
            _battleMenuController = stateMachine.BattleMenuController;
        }
        
        public override void OnEnter()
        {
            base.OnEnter();
            _battleMenuFSM.ActiveState = this;
        }
    }
}