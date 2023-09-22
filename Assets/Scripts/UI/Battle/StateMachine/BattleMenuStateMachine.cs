using CryptoQuest.PushdownFSM;
using CryptoQuest.UI.Battle.StartBattle;

namespace CryptoQuest.UI.Battle.StateMachine
{
    public class BattleMenuStateMachine : PushdownStateMachine
    {
        public static readonly string SelectSingleEnemyState = "SelectSingleEnemy";
        public static readonly string SelectEnemyGroupState = "SelectEnemyGroup";
        public static readonly string SelectSkillState = "SelectSkill";
        public static readonly string SelectItemState = "SelectItem";
        public static readonly string SelectHeroState = "SelectHero";

        public BattleMenuController BattleMenuController { get; }

        public new BattleMenuStateBase ActiveState { get; set; }

        public BattleMenuStateMachine(BattleMenuController controller) : base(false)
        {
            BattleMenuController = controller;
            
            AddBattleMenuStates();

            // TODO: Setup and init other states here
            AddState(SelectSingleEnemyState, new SelectSingleEnemyState(this));
            AddState(SelectEnemyGroupState, new SelectEnemyGroupState(this));
            AddState(SelectSkillState, new SelectSkillState(this));
            AddState(SelectItemState, new SelectItemState(this));

            SetStartState(UIStartBattle.StartBattleState);
            Init();
        }

        private void AddBattleMenuStates()
        {
            var uis = BattleMenuController.GetComponentsInChildren<IBattleMenu>(true);
            foreach (var ui in uis)
            {
                var uiState = ui.CreateState(this);
                AddState(ui.StateName, uiState);
            }
        }
    }
}