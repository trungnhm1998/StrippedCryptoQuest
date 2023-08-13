using CryptoQuest.UI.Menu.ScriptableObjects;
using FSM;
using UnityEngine;

namespace CryptoQuest.UI.Menu.Panels
{
    public abstract class UIMenuPanel : MonoBehaviour
    {
        [SerializeField] private MenuTypeSO _typeSO;
        public MenuTypeSO TypeSO => _typeSO;

        [SerializeField] protected GameObject _content;

        public void Show()
        {
            _content.SetActive(true);
            OnShow();
        }

        // TODO: Rename method, I followed Unity's naming convention
        protected virtual void OnShow() {}

        public void Hide()
        {
            _content.SetActive(false);
            OnHide();
        }
        
        // TODO: Rename method, I followed Unity's naming convention
        protected virtual void OnHide() {}

        /// <summary>
        /// Act as a factory method to create the state machine for each panel.
        /// Panel that inherits from this class should implement this method, and return the correct state machine.
        /// Inside that state machine, it should have all the states that the panel can have. Usually in constructor.
        /// </summary>
        /// <param name="menuManager">The panel should knows about the MenuManager so its can interact with the nav bar</param>
        /// <returns><see cref="CryptoQuest.UI.Menu.MenuStates.MenuStateMachine"/> for the panel </returns>
        public abstract StateBase<string> GetPanelState(MenuManager menuManager);
    }
}