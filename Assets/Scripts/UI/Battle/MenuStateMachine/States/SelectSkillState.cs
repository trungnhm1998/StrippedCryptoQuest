using CryptoQuest.UI.Battle.CommandsMenu;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Skills;
using IndiGames.GameplayAbilitySystem.AbilitySystem;

namespace CryptoQuest.UI.Battle.MenuStateMachine.States
{
    public class SelectSkillState : SelectStateBase
    {
        public SelectSkillState(BattleMenuStateMachine stateMachine) : base(stateMachine) { }

        protected override void SetupButtonsInfo()
        {
            foreach (var abstractAbility in _currentUnit.Owner.GrantedAbilities.Abilities)
            {
                var abilitySO = abstractAbility.AbilitySO;
                var isAtivableSkill = _battlePanelController.TagConfigSO.CheckNotActivableSkillTag(abilitySO.Tags.AbilityTag);
                if (abilitySO is not AbilitySO || isAtivableSkill) continue;

                var buttonInfo = new AbilityAbstractButtonInfo(SetAbility, abstractAbility);
                _buttonInfos.Add(buttonInfo);
            }
        }

        private void SetAbility(AbstractAbility ability)
        {
            _currentUnit.SelectAbility(ability);
            HandleTarget();
        }
    }
}