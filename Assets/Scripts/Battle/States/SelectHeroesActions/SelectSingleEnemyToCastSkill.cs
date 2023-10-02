using CryptoQuest.Battle.Commands;
using CryptoQuest.Battle.Components;
using CryptoQuest.Battle.UI.CommandDetail;
using CryptoQuest.Battle.UI.SelectSkill;
using UnityEngine.InputSystem;

namespace CryptoQuest.Battle.States.SelectHeroesActions
{
    public class SelectSingleEnemyToCastSkill : StateBase
    {
        private readonly UISkill _selectedSkill;
        private readonly SelectSkillPresenter _skillPresenter;
        private readonly SelectEnemyPresenter _selectEnemyPresenter;

        public SelectSingleEnemyToCastSkill(UISkill selectedSkill, HeroBehaviour hero, SelectHeroesActions fsm) :
            base(hero, fsm)
        {
            Fsm.TryGetComponent(out _skillPresenter);
            _selectedSkill = selectedSkill;
            _selectEnemyPresenter = fsm.BattleStateMachine.gameObject.GetComponentInChildren<SelectEnemyPresenter>();
        }

        public override void OnEnter()
        {
            _skillPresenter.Hide();
            _selectEnemyPresenter.Show();
            _selectEnemyPresenter.RegisterEnemySelectedCallback(CreateCommandToCastSkillOnEnemy);
            Fsm.BattleStateMachine.BattleInput.InputActions.BattleMenu.Cancel.performed += CancelPressed;
        }

        public override void OnExit()
        {
            Fsm.BattleStateMachine.BattleInput.InputActions.BattleMenu.Cancel.performed -= CancelPressed;
            _selectEnemyPresenter.Hide();
        }

        private void CancelPressed(InputAction.CallbackContext obj)
        {
            if (obj.performed) Fsm.PopState();
        }

        private void CreateCommandToCastSkillOnEnemy(EnemyBehaviour enemy)
        {
            var castSkillCommand =
                new CastSkillCommand(Hero.CharacterComponent, _selectedSkill.Skill, enemy.CharacterComponent);
            Hero.TryGetComponent(out Components.Character character);
            character.SetCommand(castSkillCommand);
            Fsm.GoToNextState();
        }
    }
}