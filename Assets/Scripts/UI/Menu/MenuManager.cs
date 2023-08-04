using CryptoQuest.Input;
using CryptoQuest.UI.Menu.MenuStates;
using CryptoQuest.UI.Menu.Panels;
using CryptoQuest.UI.Menu.ScriptableObjects;
using UnityEngine;
using NotImplementedException = System.NotImplementedException;

namespace CryptoQuest.UI.Menu
{
    public class MenuManager : MonoBehaviour
    {
        [Header("Configs")]
        [SerializeField] private InputMediatorSO _inputMediator;

        [Header("Game Components")] [SerializeField]
        private GameObject _contents;

        [SerializeField] private UINavigationBar _navigationBar;
        public UINavigationBar NavigationBar => _navigationBar;
        [SerializeField] private MenuTypeSO _defaultMenu;
        [SerializeField] private GameObject _panelsContainer;

        private MainMenuStateMachine _mainMenuFsm;

        private void Awake()
        {
            SetupMenuStateMachines();
        }

        /// <summary>
        /// Find all the UIMenuPanel in <see cref="_panelsContainer"/> and setup the state machines.
        /// Through Polymorphism <see cref="UIMenuPanel.GetPanelState"/>, each
        /// UIMenuPanel needs to implement and return correct state machine.
        ///
        /// State machine will be init when the main menu first open at <see cref="ShowMainMenu"/>.
        /// <seealso cref="CryptoQuest.UI.Menu.Panels.Status.UIStatusMenu.GetPanelState"/>
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
            _navigationBar.MenuChanged += ChangeMenu;

            _inputMediator.ShowMainMenu += ShowMainMenu; // Start Button
            _inputMediator.StartPressed += CloseMainMenu; // also start button but only in main menu
            _inputMediator.MenuCancelEvent += MenuCancelEventRaised; // East Button
            _inputMediator.TabChangeEvent += ChangeTab; // shoulder LB/RB
            _inputMediator.MenuSubmitEvent += Submit; // South Button
            _inputMediator.MenuInteractEvent += Interact;
        }

        private void OnDisable()
        {
            _navigationBar.MenuChanged -= ChangeMenu;

            _inputMediator.ShowMainMenu -= ShowMainMenu;
            _inputMediator.StartPressed -= CloseMainMenu; // also start button but only in main menu
            _inputMediator.MenuCancelEvent -= MenuCancelEventRaised;
            _inputMediator.TabChangeEvent -= ChangeTab;
            _inputMediator.MenuSubmitEvent -= Submit;
            _inputMediator.MenuInteractEvent -= Interact;
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

        #endregion

        /// <summary>
        /// Will open the main menu UI when <see cref="InputMediatorSO.ShowMainMenu"/> were raised.
        /// </summary>
        private void ShowMainMenu()
        {
            _contents.SetActive(true);
            _navigationBar.SetActive(true);
            _mainMenuFsm.Init();
            _inputMediator.EnableMenuInput();
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