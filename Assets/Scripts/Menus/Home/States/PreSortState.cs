using CryptoQuest.Menus.Home.UI;
using FSM;
using UnityEngine.EventSystems;

namespace CryptoQuest.Menus.Home.States
{
    public class PreSortState : StateBase
    {
        private UIHomeMenu _panel;

        public PreSortState(UIHomeMenu panel) : base(false)
        {
            _panel = panel;
        }

        public override void OnEnter()
        {
            _panel.Input.MenuCancelEvent += HandleCancel;
            _panel.Input.MenuConfirmedEvent += StartSorting;
            _panel.SortMode.EnableSelectModeAndButtons();
        }

        public override void OnExit()
        {
            _panel.SortMode.DisableButtonAndHideHints();
            _panel.Input.MenuCancelEvent -= HandleCancel;
            _panel.Input.MenuConfirmedEvent -= StartSorting;
        }

        private void HandleCancel()
        {
            _panel.SortMode.DisableButtonAndHideHints();
            _panel.SortMode.SetDefaultSelection();
            fsm.RequestStateChange(HomeMenuStateMachine.Overview);
        }

        private void StartSorting()
        {
            var card = EventSystem.current.currentSelectedGameObject.GetComponent<UICharacterCardButton>();
            card.EnableSelectingEffect();
            _panel.SortMode.ConfirmSelect(card);
            fsm.RequestStateChange(HomeMenuStateMachine.Sort);
        }
    }
}