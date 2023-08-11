using CryptoQuest.UI.Battle.CommandsMenu;
using System.Collections.Generic;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;

namespace CryptoQuest.UI.Battle.MenuStateMachine.States
{
    public class SelectEnemyGroupState : SelectStateBase
    {
        public SelectEnemyGroupState(BattleMenuStateMachine stateMachine) : base(stateMachine) { }

        protected override void SetupButtonsInfo()
        {
            var groupUnits = _currentUnit.OpponentTeam.TeamGroups.UnitsDict;
            for (var i = 0; i < groupUnits.Count; i++)
            {
                var characterDataSo = groupUnits[i][0].UnitData;
                var buttonInfo =
                    new EnemyGroupButtonInfo(characterDataSo, groupUnits[i].Count, true, i, HandleGroupSelection);
                _buttonInfos.Add(buttonInfo);
            }
        }
        
        public void HandleGroupSelection(int index)
        {
            var selectedGroupUnits = _currentUnit.OpponentTeam.TeamGroups.UnitsDict[index];
            List<AbilitySystemBehaviour> targetsSystemBehaviours = new();
            foreach (var selectedUnit in selectedGroupUnits)
            {
                targetsSystemBehaviours.Add(selectedUnit.Owner);
            }

            _currentUnit.UnitLogic.SelectTargets(targetsSystemBehaviours);
            _battlePanelController.CloseCommandDetailPanel();
        }
    }
}