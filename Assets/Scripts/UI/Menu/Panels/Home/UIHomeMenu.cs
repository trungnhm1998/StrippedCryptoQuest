using CryptoQuest.UI.Menu.MenuStates.HomeStates;
using FSM;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.UI.Menu.Panels.Home
{
    public class UIHomeMenu : UIMenuPanel
    {
        public UnityAction OpenedEvent;

        [Header("State Context")]
        [SerializeField] private UIHomeMenuSortCharacter _sortMode;
        public UIHomeMenuSortCharacter SortMode => _sortMode;

        public override StateBase<string> GetPanelState(MenuManager menuManager)
        {
            return new HomeMenuStateMachine(this);
        }

        protected override void OnShow()
        {
            OpenedEvent?.Invoke();
        }
    }
}