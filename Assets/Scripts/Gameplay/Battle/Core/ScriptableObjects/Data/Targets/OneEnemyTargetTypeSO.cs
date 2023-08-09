using System.Collections.Generic;
using CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit;
using CryptoQuest.UI.Battle;
using CryptoQuest.UI.Battle.CommandsMenu;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data.Targets
{
    [CreateAssetMenu(fileName = "OneEnemyTargetTypeSO", menuName = "Gameplay/Battle/TargetTypes/One Enemy Target Type")]
    public class OneEnemyTargetTypeSO : BattleTargetTypeSO
    {
        public override BattleTargetType GetTargetType(IBattleUnit unit, BattlePanelController battlePanelController,
            CharacterList characterList) => new OneEnemyTargetType(unit, battlePanelController, characterList);
    }

    public class OneEnemyTargetType : BattleTargetType
    {
        public OneEnemyTargetType(IBattleUnit unit, BattlePanelController battlePanelController,
            CharacterList characterList) : base(unit, battlePanelController, characterList) { }

        public override void HandleTargets()
        {
            List<AbstractButtonInfo> _buttonInfo = new();
            foreach (var opponentUnit in _unit.OpponentTeam.BattleUnits)
            {
                var buttonInfo = new SkillAbstractButtonInfo(opponentUnit, SelectTarget);
                _buttonInfo.Add(buttonInfo);
            }

            _battlePanelController.OpenCommandDetailPanel(_buttonInfo);
        }

        private void SelectTarget(IBattleUnit targetUnit)
        {
            _unit.SelectSingleTarget(targetUnit.Owner);
        }
    }
}