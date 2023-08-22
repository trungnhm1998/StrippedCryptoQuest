using FSM;
using UnityEngine;
using NotImplementedException = System.NotImplementedException;

namespace CryptoQuest.UI.Menu.MenuStates
{
    /// <summary>
    /// This is the state machine for the main menu. through this we will handle all sub states and
    /// sub state machines for each panels.
    /// </summary>
    public class MainMenuStateMachine : StateMachine
    {
        public MenuManager MenuManagerContext { get; }

        /// <summary>
        /// Using the built in ActiveState property to get the active state. so we can delegate the logic.
        /// 
        /// TODO: This is not a good way to get the active state. A better way to do this is to create a StateMachine
        /// that use <see cref="StateMachine{TOwnId, TStateId, TEvent}.StateBundle"/> but with our own implementation.
        /// That has <see cref="HandleCancel"/> and <see cref="Interact"/> method or with any methods that a menu needs,
        /// or even better, a <see cref="IMenuState"/> that should have all the methods that a menu needs.
        /// </summary>
        private new MenuStateMachine ActiveState => (MenuStateMachine)base.ActiveState; // TODO: code smell here

        public MainMenuStateMachine(MenuManager menuManagerContext) : base(false)
        {
            MenuManagerContext = menuManagerContext;
        }

        #region Specific logics to delegate

        public void HandleCancel()
        {
            ActiveState.HandleCancel();
        }

        public void Interact()
        {
            ActiveState.Interact();
        }

        public void ChangeTab(float direction)
        {
            ActiveState.ChangeTab(direction);
        }

        public void HandleNavigate(Vector2 direction)
        {
            ActiveState.HandleNavigate(direction);
        }
        public void Confirm()
        {
            ActiveState.Confirm();
        }
        #endregion

    }
}