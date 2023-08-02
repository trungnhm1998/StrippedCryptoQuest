using System.Collections.Generic;
using CryptoQuest.Events.Gameplay;
using CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Skills;
using CryptoQuest.UI.Battle.CommandsMenu;
using UnityEngine;

namespace CryptoQuest.UI.Battle
{
    public class SelectSkillHandler : BattleActionHandler.BattleActionHandler
    {
        [SerializeField] private BattlePanelController _panelController;

        private readonly List<AbstractButtonInfo> _buttonInfo = new();
        private IBattleUnit _currentUnit;

        public override void Handle(IBattleUnit currentUnit)
        {
            _currentUnit = currentUnit;
            SetupTargetButton(_currentUnit);
        }

        private void SetupTargetButton(IBattleUnit battleUnit)
        {
            _buttonInfo.Clear();
            foreach (var skill in _currentUnit.UnitData.GrantedSkills)
            {
                var cryptoQuestAbility = skill as AbilitySO;
                if (cryptoQuestAbility == null) continue;
                var buttonInfo = new AbilityAbstractButtonInfo(SetAbility, cryptoQuestAbility);
                _buttonInfo.Add(buttonInfo);
            }

            _panelController.OpenCommandDetailPanel(_buttonInfo);
        }

        private void SetAbility(AbilitySO abilitySO)
        {
            var ability = _currentUnit.Owner.GiveAbility(abilitySO);
            _currentUnit.SelectSkill(ability);
            _panelController.CloseCommandDetailPanel();
            NextHandler?.Handle(_currentUnit);
        }
    }
}