using System.Collections.Generic;
using CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit;
using CryptoQuest.UI.Battle;
using CryptoQuest.UI.Battle.CommandsMenu;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data.Targets
{
    [CreateAssetMenu(fileName = "OneAllyTargetTypeSO", menuName = "Gameplay/Battle/TargetTypes/OneAllyTargetTypeSO")]
    public class OneAllyTargetTypeSO : BattleTargetTypeSO
    {
        public override BattleTargetType GetTargetType(IBattleUnit unit, BattlePanelController battlePanelController,
            CharacterList characterList) => new OneAllyTargetType(unit, battlePanelController, characterList);
    }

    public class OneAllyTargetType : BattleTargetType
    {
        public OneAllyTargetType(IBattleUnit unit, BattlePanelController battlePanelController,
            CharacterList characterList) : base(unit, battlePanelController, characterList) { }

        public override void HandleTargets()
        {
            List<AbstractButtonInfo> _buttonInfo = new();

            foreach (var allyUnit in _unit.OwnerTeam.BattleUnits)
            {
                var buttonInfo = new SkillAbstractButtonInfo(allyUnit, SelectTarget);
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