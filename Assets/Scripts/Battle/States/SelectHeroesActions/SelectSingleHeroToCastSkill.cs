using CryptoQuest.Battle.Commands;
using CryptoQuest.Battle.Components;
using CryptoQuest.Battle.UI.SelectHero;
using CryptoQuest.Battle.UI.SelectSkill;
using UnityEngine.InputSystem;

namespace CryptoQuest.Battle.States.SelectHeroesActions
{
    internal class SelectSingleHeroToCastSkill : StateBase
    {
        private readonly UISkill _selectedSkillUI;
        private readonly SelectHeroPresenter _selectHeroPresenter;
        private readonly SelectSkillPresenter _skillPresenter;

        public SelectSingleHeroToCastSkill(UISkill selectedSkillUI, HeroBehaviour hero, SelectHeroesActions fsm) :
            base(hero, fsm)
        {
            _selectedSkillUI = selectedSkillUI;
            _selectHeroPresenter = Fsm.BattleStateMachine.SelectHeroPresenter;
            Fsm.TryGetComponent(out _skillPresenter);
        }

        public override void OnEnter()
        {
            _selectHeroPresenter.Show(_selectedSkillUI.Skill.Parameters.SkillName);
            _skillPresenter.Show(Hero);
            Fsm.BattleStateMachine.BattleInput.InputActions.BattleMenu.Cancel.performed += CancelPressed;
            SelectHeroPresenter.ConfirmSelectCharacter += CastSkillOnHero;
        }

        public override void OnExit()
        {
            Fsm.BattleStateMachine.BattleInput.InputActions.BattleMenu.Cancel.performed -= CancelPressed;
            _selectHeroPresenter.Hide();
            _skillPresenter.Hide();
            SelectHeroPresenter.ConfirmSelectCharacter -= CastSkillOnHero;
        }

        private void CancelPressed(InputAction.CallbackContext obj)
        {
            if (obj.performed) Fsm.PopState();
        }

        private void CastSkillOnHero(HeroBehaviour selectedHero)
        {
            var castSkillCommand = new CastSkillCommand(
                Hero.CharacterComponent,
                _selectedSkillUI.Skill,
                selectedHero.CharacterComponent);
            Hero.TryGetComponent(out Components.Character character);
            character.SetCommand(castSkillCommand);
            Fsm.GoToNextState();
        }
    }
}