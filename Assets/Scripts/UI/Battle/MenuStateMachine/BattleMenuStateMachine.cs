using CryptoQuest.UI.Battle.MenuStateMachine.States;
using CryptoQuest.PushdownFSM;
using UnityEngine;

namespace CryptoQuest.UI.Battle.MenuStateMachine
{
    public class BattleMenuStateMachine : PushdownStateMachine
    {
        public static readonly string SelectCommandState = "SelectCommand";
        public static readonly string SelectSingleEnemyState = "SelectSingleEnemy";
        public static readonly string SelectEnemyGroupState = "SelectEnemyGroup";
        public static readonly string SelectSkillState = "SelectSkill";
        public static readonly string SelectItemState = "SelectItem";
        public static readonly string SelectHeroState = "SelectHero";

        public BattlePanelController BattlePanelController { get; }

        public new BattleMenuStateBase ActiveState { get; set; }

        public BattleMenuStateMachine(BattlePanelController controller) : base(false)
        {
            BattlePanelController = controller;
            
            AddState(SelectCommandState, new SelectCommandState(this));
            AddState(SelectSkillState, new SelectSkillState(this));
            AddState(SelectItemState, new SelectItemState(this, controller.Inventory));
            AddState(SelectSingleEnemyState, new SelectSingleEnemyState(this));
            AddState(SelectEnemyGroupState, new SelectEnemyGroupState(this));
            AddState(SelectHeroState, new SelectHeroState(this, controller.CharactersUI));

            SetStartState(SelectCommandState);
            Init();
        }

        public void HandleCancel()
        {
            PushdownState();
        }

    }
}