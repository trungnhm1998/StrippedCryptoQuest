using System;
using CryptoQuest.Input;
using CryptoQuest.UI.Menu.MenuStates;
using CryptoQuest.UI.Menu.Panels;
using CryptoQuest.UI.Menu.Panels.Status;
using CryptoQuest.UI.Menu.ScriptableObjects;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.UI.Menu
{
    [Obsolete]
    public class MenuManager : MonoBehaviour
    {
        [SerializeField] private VoidEventChannelSO _onCloseMainMenuEventChannel;

        [Header("Configs")]
        [SerializeField] private InputMediatorSO _inputMediator;
        [SerializeField] private VoidEventChannelSO _menuOpenedEvent;

        [Header("Game Components")]
        [SerializeField] private GameObject _contents;
        [SerializeField] private UINavigationBar _navigationBar;
        public UINavigationBar NavigationBar => _navigationBar;
        [SerializeField] private MenuTypeSO _defaultMenu;
        [SerializeField] private GameObject _panelsContainer;

        private MainMenuStateMachine _mainMenuFsm;
        public MainMenuStateMachine MainMenuFsm => _mainMenuFsm;

        private void Awake()
        {
            SetupMenuStateMachines();
        }

        /// <summary>
        /// Find all the UIMenuPanel in <see cref="_panelsContainer"/> and setup the state machines.
        /// Through Polymorphism <see cref="UIMenuPanel.GetPanelState"/>, each
        /// UIMenuPanel needs to implement and return correct state machine.
        ///
        /// State machine will be init when the main menu first open at <see cref="ShowMainMenuEvent"/>.
        /// <seealso cref="UIStatusMenu.GetPanelState"/>
        /// </summary>
        private void SetupMenuStateMachines()
        {
            _mainMenuFsm = new MainMenuStateMachine(this);
            var panels = _panelsContainer.GetComponentsInChildren<UIMenuPanel>();
            foreach (var panel in panels)
            {
                _mainMenuFsm.AddState(panel.TypeSO.name, panel.GetPanelState(this));
            }

            _mainMenuFsm.SetStartState(_defaultMenu.name);
        }

        /// <summary>
        /// For each new interaction with the menu, we will register the new events here and delegates the logic to the
        /// state machine
        /// </summary>
        private void OnEnable()
        {
            if (_mainMenuFsm != null)
            {
                _inputMediator.MenuNavigateEvent += _mainMenuFsm.HandleNavigate;
                _inputMediator.MenuConfirmedEvent += _mainMenuFsm.Confirm;
            }
            _onCloseMainMenuEventChannel.EventRaised += CloseMainMenu;
            _navigationBar.MenuChanged += ChangeMenu;
            _inputMediator.ShowMainMenuEvent += ShowMainMenuEvent; // Start Button
            _inputMediator.CloseMainMenuEvent += CloseMainMenu; // also start button but only in main menu
            _inputMediator.MenuCancelEvent += MenuCancelEventRaised; // East Button
            _inputMediator.TabChangeEvent += ChangeTab; // shoulder LB/RB
            _inputMediator.MenuSubmitEvent += Submit; // South Button
            _inputMediator.MenuInteractEvent += Interact;
            _inputMediator.MenuResetEvent += Reset;
            _inputMediator.MenuExecuteEvent += Execute;
        }

        private void OnDisable()
        {
            _onCloseMainMenuEventChannel.EventRaised -= CloseMainMenu;
            _navigationBar.MenuChanged -= ChangeMenu;

            _inputMediator.ShowMainMenuEvent -= ShowMainMenuEvent;
            _inputMediator.CloseMainMenuEvent -= CloseMainMenu; // also start button but only in main menu
            _inputMediator.MenuCancelEvent -= MenuCancelEventRaised;
            _inputMediator.TabChangeEvent -= ChangeTab;
            _inputMediator.MenuSubmitEvent -= Submit;
            _inputMediator.MenuInteractEvent -= Interact;

            _inputMediator.MenuResetEvent -= Reset;
            _inputMediator.MenuExecuteEvent -= Execute;

            if (_mainMenuFsm != null)
            {
                _inputMediator.MenuConfirmedEvent -= _mainMenuFsm.Confirm;
                _inputMediator.MenuNavigateEvent -= _mainMenuFsm.HandleNavigate;
                _mainMenuFsm.OnExit();
            }
        }

        #region State Machine Delegates

        /// <summary>
        /// When presses the "E" button on keyboard, each panels (sub state machine) will have its own logic to handle
        /// delegate the logic to the main menu state machine. first then it will delegate to the sub state machine.
        /// </summary>
        private void Interact()
        {
            _mainMenuFsm.Interact();
        }

        /// <summary>
        /// Delegate the logic for sub states and sub state machines.
        /// Some state will called the CloseMainMenu() method to close the menu. Usually at State suffix with Navigation.
        /// </summary>
        private void MenuCancelEventRaised()
        {
            _mainMenuFsm.HandleCancel();
        }

        private void ChangeTab(float direction)
        {
            _mainMenuFsm.ChangeTab(direction);
        }

        private void Submit()
        {
            _mainMenuFsm.OnLogic();
        }

        private void Reset()
        {
            _mainMenuFsm.Reset();
        }

        private void Execute()
        {
            _mainMenuFsm.Execute();
        }

        #endregion

        /// <summary>
        /// Will open the main menu UI when <see cref="InputMediatorSO.ShowMainMenuEvent"/> were raised.
        /// </summary>
        private void ShowMainMenuEvent()
        {
            _contents.SetActive(true);
            _navigationBar.SetActive(true);
            _mainMenuFsm.Init();
            _inputMediator.EnableMenuInput();
            _menuOpenedEvent.RaiseEvent();
        }

        /// <summary>
        /// Destructive method to close the main menu UI.
        /// </summary>
        public void CloseMainMenu()
        {
            _navigationBar.SetActive(false);
            _contents.SetActive(false);
            _inputMediator.EnableMapGameplayInput();
        }

        private void ChangeMenu(MenuTypeSO typeSO)
        {
            Debug.Log($"ChangeMenu {typeSO.name}");
            _mainMenuFsm.RequestStateChange(typeSO.name);
        }
    }
}