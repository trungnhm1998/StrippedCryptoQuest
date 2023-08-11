using UnityEngine.EventSystems;

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
            //TODO: Refactor ability name later (item name for item and show display ability name)
            _charactersUI.SetSelectedData(_currentUnit.UnitLogic.SelectedAbility.AbilitySO.name);
            _charactersUI.SelectFirstHero();

            _battlePanelController.SetActiveCommandDetailButtons(false);
        }

        public override void OnExit()
        {
            _battlePanelController.SetActiveCommandDetailButtons(true);
            base.OnExit();
        }
    }
}