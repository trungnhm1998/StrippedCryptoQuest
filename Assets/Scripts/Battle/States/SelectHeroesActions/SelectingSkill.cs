using CryptoQuest.Battle.Components;
using CryptoQuest.Battle.UI.CommandDetail;
using CryptoQuest.Battle.UI.SelectSkill;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CryptoQuest.Battle.States.SelectHeroesActions
{
    public class SelectingSkill : StateBase
    {
        private readonly SelectSkillPresenter _skillPresenter;
        private readonly SelectEnemyPresenter _selectEnemyPresenter;

        public SelectingSkill(HeroBehaviour hero, SelectHeroesActions fsm) : base(hero, fsm)
        {
            Fsm.TryGetComponent(out _skillPresenter);
            _selectEnemyPresenter = fsm.BattleStateMachine.gameObject.GetComponentInChildren<SelectEnemyPresenter>();
        }

        public override void OnEnter()
        {
            _skillPresenter.Show(Hero);
            _skillPresenter.SelectSingleEnemyCallback = SelectEnemyToCastSkillOn;
            _skillPresenter.SelectSingleHeroCallback = SelectHeroToCastSkillOn;
            RegisterEvents();
        }

        public override void OnExit()
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
            Fsm.PopState();
        }

        private void SelectEnemyToCastSkillOn(UISkill skillUI)
        {
            Fsm.PushState(new SelectSingleEnemyToCastSkill(skillUI, Hero, Fsm));
            Debug.Log("SelectingSkill::SelectEnemyToCastSkillOn");
        }

        private void SelectHeroToCastSkillOn(UISkill skillUI)
        {
            Debug.Log("SelectingSkill::SelectHeroToCastSkillOn");
        }
    }
}