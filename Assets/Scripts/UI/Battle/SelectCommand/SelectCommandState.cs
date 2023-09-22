using UnityEngine;
using CryptoQuest.UI.Battle.StateMachine;

namespace CryptoQuest.UI.Battle.SelectCommand
{
    public class SelectCommandState : BattleMenuStateBase
    {
        private UISelectCommand _selectCommandUI;

        public SelectCommandState(BattleMenuStateMachine stateMachine, UISelectCommand selectCommandUI)
            : base(stateMachine)
        {
            _selectCommandUI = selectCommandUI;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _selectCommandUI.SetActiveCommandsMenu(true);
            _selectCommandUI.SelectFirstButton();
        }

        public override void OnExit()
        {
            base.OnExit();
            _selectCommandUI.SetActiveCommandsMenu(false);
        }

        public void ChangeToSkillState()
        {
            _battleMenuFSM.RequestStateChange(BattleMenuStateMachine.SelectSkillState);
        }

        public void ChangeToItemState()
        {
            _battleMenuFSM.RequestStateChange(BattleMenuStateMachine.SelectItemState);
        }

    }
}