using CryptoQuest.UI.Battle.StateMachine;

namespace CryptoQuest.UI.Battle.StartBattle
{
    public class StartBattleState : BattleMenuStateBase
    {
        private UIStartBattle _startBattleUI;

        public StartBattleState(BattleMenuStateMachine stateMachine, 
            UIStartBattle startBattleUI) : base(stateMachine)
        {
            _startBattleUI = startBattleUI;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _startBattleUI.EnterStartBattleState();
        }
    }
}