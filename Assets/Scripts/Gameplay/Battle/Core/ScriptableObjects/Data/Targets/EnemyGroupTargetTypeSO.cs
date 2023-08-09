using System.Collections.Generic;
using CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit;
using CryptoQuest.UI.Battle;
using CryptoQuest.UI.Battle.CommandsMenu;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using UnityEditor.Localization.Plugins.XLIFF.V20;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data.Targets
{
    public class EnemyGroupTargetTypeSO : BattleTargetTypeSO
    {
        public override BattleTargetType GetTargetType(IBattleUnit unit, BattlePanelController battlePanelController,
            CharacterList characterList) => new EnemyGroupTargetType(unit, battlePanelController, characterList);
    }

    public class EnemyGroupTargetType : BattleTargetType
    {
        private BattleUnitBase unitBase;

        public EnemyGroupTargetType(IBattleUnit unit, BattlePanelController battlePanelController,
            CharacterList characterList) : base(unit, battlePanelController, characterList) { }

        public override void HandleTargets()
        {
            if (!_unit.Owner.TryGetComponent<BattleUnitBase>(out unitBase)) return;
            List<AbstractButtonInfo> abstractButtonInfos = new();
            var groupUnits = _unit.OpponentTeam.TeamGroups.UnitsDict;
            for (int i = 0; i < groupUnits.Count; i++)
            {
                CharacterDataSO characterDataSo = groupUnits[i][0].UnitData;
                var buttonInfo =
                    new EnemyGroupButtonInfo(characterDataSo, groupUnits[i].Count, true, i, HandleGroupSelection);
                abstractButtonInfos.Add(buttonInfo);
            }

            _battlePanelController.OpenCommandDetailPanel(abstractButtonInfos);
        }

        public void HandleGroupSelection(int index)
        {
            var selectedGroupUnits = _unit.OpponentTeam.TeamGroups.UnitsDict[index];
            List<AbilitySystemBehaviour> targetsSystemBehaviours = new();
            foreach (var selectedUnit in selectedGroupUnits)
            {
                targetsSystemBehaviours.Add(selectedUnit.Owner);
            }

            unitBase.SelectTargets(targetsSystemBehaviours);
        }
    }
}