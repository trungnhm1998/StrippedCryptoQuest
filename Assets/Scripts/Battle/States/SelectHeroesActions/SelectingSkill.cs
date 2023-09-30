using CryptoQuest.Battle.Commands;
using CryptoQuest.Battle.Components;
using CryptoQuest.Battle.UI.CommandDetail;
using CryptoQuest.Battle.UI.SelectSkill;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CryptoQuest.Battle.States.SelectHeroesActions
{
    public class SelectingSkill : StateBase
    {
        private SelectSkillPresenter _skillPresenter;
        private readonly SelectEnemyPresenter _selectEnemyPresenter;

        public SelectingSkill(HeroBehaviour hero, SelectHeroesActions fsm) : base(hero, fsm)
        {
            _selectEnemyPresenter = fsm.BattleStateMachine.gameObject.GetComponentInChildren<SelectEnemyPresenter>();
        }

        public override void OnEnter()
        {
            Fsm.TryGetComponent(out _skillPresenter);
            _skillPresenter.Show(Hero);
            _skillPresenter.SelectSingleEnemyCallback = SelectEnemyToCastSkillOn;
            _skillPresenter.SelectSingleHeroCallback = SelectHeroToCastSkillOn;
            RegisterEvents();
        }

        public override void OnExit() => OnDestroy();

        public override void OnDestroy()
        {
            _skillPresenter.Hide();
            _selectEnemyPresenter.Hide();
            UnregisterEvents();
        }

        private void RegisterEvents()
        {
            Fsm.BattleStateMachine.BattleInput.InputActions.BattleMenu.Cancel.performed += CancelPressed;
        }

        private void UnregisterEvents()
        {
            Fsm.BattleStateMachine.BattleInput.InputActions.BattleMenu.Cancel.performed -= CancelPressed;
        }

        private void CancelPressed(InputAction.CallbackContext obj)
        {
            if (_flag)
            {
                _selectEnemyPresenter.Hide();
                _skillPresenter.Show(Hero);
                _flag = false;
                return;
            }

            Fsm.PopState();
        }

        private bool _flag; // TODO: implement state machine for this
        private UISkill _selectedSkillUI;

        private void SelectEnemyToCastSkillOn(UISkill skillUI)
        {
            _selectedSkillUI = skillUI;
            _flag = true;
            Debug.Log("SelectingSkill::SelectEnemyToCastSkillOn");
            _skillPresenter.Hide();
            _selectEnemyPresenter.Show();
            _selectEnemyPresenter.RegisterEnemySelectedCallback(CreateCommandToCastSkillOnEnemy);
        }

        private void CreateCommandToCastSkillOnEnemy(EnemyBehaviour enemy)
        {
            var castSkillCommand = new CastSkillCommand(Hero, _selectedSkillUI.Skill, enemy);
            Hero.TryGetComponent(out Components.Character character);
            character.SetCommand(castSkillCommand);
            Fsm.GoToNextState();
        }

        private void SelectHeroToCastSkillOn(UISkill skillUI)
        {
            Debug.Log("SelectingSkill::SelectHeroToCastSkillOn");
        }
    }
}