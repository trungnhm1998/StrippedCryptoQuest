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
        private readonly UISelectHeroButton _selectHeroUI;
        private readonly SelectSkillPresenter _skillPresenter;

        public SelectSingleHeroToCastSkill(UISkill selectedSkillUI, HeroBehaviour hero, SelectHeroesActions fsm) :
            base(hero, fsm)
        {
            _selectedSkillUI = selectedSkillUI;
            _selectHeroPresenter = Fsm.BattleStateMachine.SelectHeroPresenter;
            _selectHeroUI = _selectHeroPresenter.GetComponent<UISelectHeroButton>();
            Fsm.TryGetComponent(out _skillPresenter);
        }

        public override void OnEnter()
        {
            _skillPresenter.Show(Hero);
            _selectHeroUI.SetUIActive(true);
            Fsm.BattleStateMachine.BattleInput.InputActions.BattleMenu.Cancel.performed += CancelPressed;
        }

        public override void OnExit()
        {
            Fsm.BattleStateMachine.BattleInput.InputActions.BattleMenu.Cancel.performed -= CancelPressed;
            _selectHeroUI.SetUIActive(false);
        }

        private void CancelPressed(InputAction.CallbackContext obj)
        {
            if (obj.performed) Fsm.PopState();
        }
    }
}