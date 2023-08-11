using CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit;
using CryptoQuest.UI.Battle.CommandsMenu;

namespace CryptoQuest.UI.Battle.MenuStateMachine.States
{
    public class SelectSingleEnemyState : SelectStateBase
    {
        public SelectSingleEnemyState(BattleMenuStateMachine stateMachine) : base(stateMachine) { }

        protected override void SetupButtonsInfo()
        {
            var targetUnits = _currentUnit.OpponentTeam.BattleUnits;

            foreach (var unit in targetUnits)
            {
                var buttonInfo = new SkillAbstractButtonInfo(unit, SelectTarget);
                _buttonInfos.Add(buttonInfo);
            }
        }
        
        private void SelectTarget(IBattleUnit unit)
        {
            _currentUnit.SelectSingleTarget(unit.Owner);
            _battlePanelController.CloseCommandDetailPanel();
        }
    }
}