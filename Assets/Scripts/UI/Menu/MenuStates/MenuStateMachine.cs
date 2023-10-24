using CryptoQuest.UI.Menu.Panels;
using FSM;
using UnityEngine;

namespace CryptoQuest.UI.Menu.MenuStates
{
    /// <summary>
    /// For each menu panel, there is a state machine. Base logic was when enter a State Machine for specific panel,
    /// we enable the contents of that panel, and when exit, we disable the contents.
    /// Usually from Home <-> Status <-> Inventory <-> Shop
    /// Within the State Machine, there are states, which is the actual logic of the panel.
    /// For example, Status panel has 3 states: Status, Equipment, EquipmentSelection.
    /// The <see cref="OnEnter"/> will delegate to the active state, and call <see cref="MenuStateBase.OnEnter"/>
    /// </summary>
    public abstract class MenuStateMachine : StateMachine
    {
        private readonly UIMenuPanel _panel;
        protected MainMenuStateMachine SuperFsm => (MainMenuStateMachine)fsm; // TODO: code smell here too

        public MenuManager MenuManager => SuperFsm.MenuManagerContext;

        private new MenuStateBase ActiveState => (MenuStateBase)base.ActiveState; // TODO: code smell here

        protected MenuStateMachine(UIMenuPanel panel) : base(false)
        {
            _panel = panel;
        }

        /// <summary>
        /// ORDER IS MATTER HERE
        /// Show the panel first, then call base.OnEnter() to delegate to the active state.
        /// </summary>
        public override void OnEnter()
        {
            _panel.Show();
            base.OnEnter();
        }

        public override void OnExit()
        {
            base.OnExit();
            _panel.Hide();
        }

        public virtual void HandleCancel()
        {
            ActiveState.HandleCancel();
        }

        public virtual void Interact()
        {
            ActiveState.Interact();
        }

        public virtual void Confirm()
        {
            ActiveState.Confirm();
        }

        public virtual void ChangeTab(float direction)
        {
            ActiveState.ChangeTab(direction);
        }

        public virtual void HandleNavigate(Vector2 direction)
        {
            ActiveState.HandleNavigate(direction);
        }

        public virtual void Reset()
        {
            ActiveState.Reset();
        }

        /// <summary>
        /// Use this method to execute actions that are important.
        /// </summary>
        public virtual void Execute()
        {
            ActiveState.Execute();
        }
    }
}