using CryptoQuest.Battle.Components;
using CryptoQuest.Battle.UI.SelectSkill;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CryptoQuest.Battle.States.SelectHeroesActions
{
    public class SelectingSkill : StateBase
    {
        private readonly SelectSkillPresenter _skillPresenter;

        public SelectingSkill(HeroBehaviour hero, SelectHeroesActions fsm) : base(hero, fsm)
        {
            Fsm.TryGetComponent(out _skillPresenter);
        }

        public override void OnEnter()
        {
            _skillPresenter.Interactable = true;
            _skillPresenter.Show(Hero);
            _skillPresenter.SelectSingleEnemyCallback = SelectEnemyToCastSkillOn;
            _skillPresenter.SelectSingleHeroCallback = SelectHeroToCastSkillOn;
            RegisterEvents();
        }

        public override void OnExit()
        {
            UnregisterEvents();
            _skillPresenter.Hide();
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
            if (obj.performed) Fsm.PopState();
        }

        private void SelectEnemyToCastSkillOn(UISkill skillUI)
        {
            _skillPresenter.Interactable = false;
            Debug.Log("SelectingSkill::SelectEnemyToCastSkillOn");
            Fsm.PushState(new SelectSingleEnemyToCastSkill(skillUI, Hero, Fsm));
        }

        private void SelectHeroToCastSkillOn(UISkill skillUI)
        {
            _skillPresenter.Interactable = false;
            Debug.Log("SelectingSkill::SelectHeroToCastSkillOn");
            Fsm.PushState(new SelectSingleHeroToCastSkill(skillUI, Hero, Fsm));
        }
    }
}