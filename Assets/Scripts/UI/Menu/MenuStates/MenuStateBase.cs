using FSM;
using UnityEngine;

namespace CryptoQuest.UI.Menu.MenuStates
{
    /// <summary>
    /// Making this abstract might be better, but I need an empty state for fast prototyping.
    /// </summary>
    public class MenuStateBase : StateBase
    {
        protected MenuStateMachine MenuStateMachine => (MenuStateMachine)fsm; // TODO: code smell here
        protected MenuManager MainMenuContext => MenuStateMachine.MenuManager;
        protected UINavigationBar NavigationBar => MenuStateMachine.MenuManager.NavigationBar;

        public MenuStateBase() : base(false) { }

        public override void OnEnter()
        {
            base.OnEnter();
            Debug.Log($"{GetType().Name}/OnEnter");
        }

        public override void OnExit()
        {
            base.OnExit();
            Debug.Log($"{GetType().Name}/OnExit");
        }

        /// <summary>
        /// Handle pressing East button on the controller or Esc button on the keyboard.
        /// </summary>
        public virtual void HandleCancel()
        {
            Debug.Log($"{GetType().Name}/HandleCancel");
        }

        /// <summary>
        /// Handle pressing West button on the controller or F button on the keyboard.
        /// </summary>
        public virtual void Interact()
        {
            Debug.Log($"{GetType().Name}/Interact");
        }

        /// <summary>
        /// Handle pressing South button on the controller or Space/Enter button on the keyboard.
        /// </summary>
        public virtual void Confirm()
        {
            Debug.Log($"{GetType().Name}/Confirm");
        }

        public virtual void ChangeTab(float direction)
        {
            Debug.Log($"{GetType().Name}/ChangeTab/{direction}");
        }

        public virtual void HandleNavigate(Vector2 direction)
        {
            Debug.Log($"{GetType().Name}/HandleNavigate/{direction}");
        }

        /// <summary>
        /// Handle pressing North button on the controller or R button on the keyboard.
        /// </summary>
        public virtual void Reset() { }

        /// <summary>
        /// Use this method to execute actions that are important.
        /// <para/>
        /// Handle pressing Menu button on the controller or C button on the keyboard.
        /// </summary>
        public virtual void Execute() { }
    }
}