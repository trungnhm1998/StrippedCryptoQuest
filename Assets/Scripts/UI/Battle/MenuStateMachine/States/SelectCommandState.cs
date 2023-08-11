using System.Collections.Generic;
using CryptoQuest.UI.Battle.CommandsMenu;

namespace CryptoQuest.UI.Battle.MenuStateMachine.States
{
    public class SelectCommandState : BattleMenuStateBase
    {
        private readonly List<AbstractButtonInfo> _enemyGroupInfos = new();
        
        public SelectCommandState(BattleMenuStateMachine stateMachine) : base(stateMachine) { }

        public override void OnEnter()
        {
            base.OnEnter();
            _battlePanelController.ReinitializeUI();
            ShowEnemyGroups();
        }

        private void ShowEnemyGroups()
        {
            _enemyGroupInfos.Clear();
            var opponentTeam = _battlePanelController.BattleManager.BattleTeam2;

            foreach (var unitPair in opponentTeam.TeamGroups.UnitsDict)
            {
                var units = unitPair.Value;
                _enemyGroupInfos.Add(new EnemyGroupButtonInfo(units[0].UnitData, units.Count, false));
            }

            _battlePanelController.OpenCommandDetailPanel(_enemyGroupInfos);
        }

        public override void OnExit()
        {
            _battlePanelController.SetActiveCommandMenu(false);
            base.OnExit();
        }
    }
}