using System.Collections.Generic;
using CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Skills;
using CryptoQuest.UI.Battle.CommandsMenu;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.UI.Battle
{
    public class SelectPlayerHandler : BattleActionHandler.BattleActionHandler
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
            var skill = _currentUnit.UnitLogic.SelectedAbility.AbilitySO as AbilitySO;
            var targetTypeSo = skill.SkillInfo.TargetType;

            var targetType = targetTypeSo.GetTargetType(battleUnit, _panelController, _characterList);
            targetType.HandleTargets();
        }
    }
}