using System.Collections.Generic;
using CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Skills;
using CryptoQuest.UI.Battle.CommandsMenu;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
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
            foreach (var abstractAbility in _currentUnit.Owner.GrantedAbilities.Abilities)
            {
                if (!(abstractAbility.AbilitySO is AbilitySO)) continue;

                var buttonInfo = new AbilityAbstractButtonInfo(SetAbility, abstractAbility);
                _buttonInfo.Add(buttonInfo);
            }

            _panelController.OpenCommandDetailPanel(_buttonInfo);
        }

        private void SetAbility(AbstractAbility ability)
        {
            _currentUnit.SelectAbility(ability);
            _panelController.CloseCommandDetailPanel();
            NextHandler?.Handle(_currentUnit);
        }
    }
}