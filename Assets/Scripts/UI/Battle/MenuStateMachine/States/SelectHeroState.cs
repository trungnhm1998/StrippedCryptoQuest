using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Skills;

namespace CryptoQuest.UI.Battle.MenuStateMachine.States
{
    public class SelectHeroState : BattleMenuStateBase
    {
        private readonly CharacterList _charactersUI;

        public SelectHeroState(BattleMenuStateMachine stateMachine, CharacterList charactersUI) : base(stateMachine)
        {
            _charactersUI = charactersUI;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            
            var selectedAbility = _currentUnit.UnitLogic.SelectedAbility;
            var ability = selectedAbility.AbilitySO as AbilitySO;
            var abilityName = ability.name;
            if (ability != null)
            {
                abilityName = ability.SkillInfo.SkillName.GetLocalizedString();
            }
            _charactersUI.SetSelectActive(true);
            _charactersUI.SetSelectedData(abilityName);
            _charactersUI.SelectFirstCharacter();

            _battlePanelController.SetActiveCommandDetailButtons(false);
        }

        public override void OnExit()
        {
            _charactersUI.SetSelectActive(false);
            _battlePanelController.SetActiveCommandDetailButtons(true);
            base.OnExit();
        }
    }
}