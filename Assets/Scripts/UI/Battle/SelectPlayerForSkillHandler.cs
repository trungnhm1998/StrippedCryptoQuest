using System.Collections.Generic;
using CryptoQuest.Events.Gameplay;
using CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Skills;
using CryptoQuest.UI.Battle.CommandsMenu;
using UnityEngine;

namespace CryptoQuest.UI.Battle
{
    public class SelectPlayerForSkillHandler : BattleActionHandler.BattleActionHandler
    {
        [SerializeField] private BattlePanelController _panelController;
        [SerializeField] private CharacterList _characterList;
        private IBattleUnit _currentUnit;
        private readonly List<AbstractButtonInfo> _buttonInfo = new();

        public override void Handle(IBattleUnit currentUnit)
        {
            _currentUnit = currentUnit;
            SetupTargetButton(_currentUnit);
        }

        private void SetupTargetButton(IBattleUnit battleUnit)
        {
            _buttonInfo.Clear();
            battleUnit.Owner.TryGetComponent<BattleUnitBase>(out var unitBase);
            var ability = unitBase.SelectedSkill.AbilitySO as AbilitySO;
            if (ability == null) return;

            var targetTypeSo = ability.SkillInfo.TargetType;
            var targetType = targetTypeSo.GetTargetType(battleUnit, _panelController, _characterList);
            targetType.HandleTargets();
        }
    }
}