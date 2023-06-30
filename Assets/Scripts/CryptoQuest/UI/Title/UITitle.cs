using Core.Runtime.Events.ScriptableObjects;
using Core.Runtime.SceneManagementSystem.Events.ScriptableObjects;
using Core.Runtime.SceneManagementSystem.ScriptableObjects;
using CryptoQuest.Input;
using CryptoQuest.UI.Menu;
using UnityEngine;

namespace CryptoQuest.UI
{
    public enum ETitleState
    {
        Waiting = 0,
        Validation = 1,
        Valid = 2,
        StartGame = 3
    }

    public class UITitle : MonoBehaviour
    {
        [SerializeField] private UINameInputPanel _nameInputPanel;
        [SerializeField] private UIInputConfirmPanel _inputConfirmPanel;

        [Space(10), SerializeField] private SceneScriptableObject _sceneToLoad;
        [SerializeField] private InputMediatorSO _inputMediatorSO;

        [Header("Listen on")]
        [SerializeField] private VoidEventChannelSO _sceneLoaded;

        [Header("Raise on")]
        [SerializeField] private LoadSceneEventChannelSO _loadMapEvent;

        private ETitleState _currentState;
        private MenuSelectionHandler _menuSelectionHandler;

        private void Awake()
        {
            _currentState = ETitleState.Waiting;
            _menuSelectionHandler = GetComponent<MenuSelectionHandler>();
        }

        private void OnEnable()
        {
            _sceneLoaded.EventRaised += SceneLoadedEvent_Raised;
            _inputMediatorSO.MenuConfirmClicked += MenuSubmitEvent_Clicked;
            _inputMediatorSO.CancelEvent += CancelEvent_Clicked;

            _nameInputPanel.OnConfirmButtonPressed += ValidateInput;
            _inputConfirmPanel.OnYesButtonPressed += StartGame;
            _inputConfirmPanel.OnNoButtonPressed += HideConfirmPanel;
        }

        private void OnDisable()
        {
            _sceneLoaded.EventRaised -= SceneLoadedEvent_Raised;
            _inputMediatorSO.MenuConfirmClicked -= MenuSubmitEvent_Clicked;
            _inputMediatorSO.CancelEvent -= CancelEvent_Clicked;

            _nameInputPanel.OnConfirmButtonPressed -= ValidateInput;
            _inputConfirmPanel.OnYesButtonPressed -= StartGame;
            _inputConfirmPanel.OnNoButtonPressed -= HideConfirmPanel;
        }


        private void CancelEvent_Clicked()
        {
            switch (_currentState)
            {
                case ETitleState.Validation:
                    _nameInputPanel.Hide();
                    _currentState = ETitleState.Waiting;
                    UpdateState();
                    break;
                case ETitleState.StartGame:
                    HideConfirmPanel();
                    break;
            }
        }

        private void MenuSubmitEvent_Clicked()
        {
            switch (_currentState)
            {
                case ETitleState.Waiting:
                    _currentState = ETitleState.Validation;
                    UpdateState();
                    return;
                case ETitleState.Validation:
                    ValidateInput();
                    return;
                case ETitleState.StartGame:
                    StartGame();
                    break;
            }
        }

        private void SceneLoadedEvent_Raised() => _inputMediatorSO.EnableMenuInput();

        public void ValidateInput()
        {
            if (!_nameInputPanel.IsValid()) return;

            _currentState = ETitleState.Valid;
            UpdateState();
        }


        private void UpdateState()
        {
            switch (_currentState)
            {
                case ETitleState.Waiting:
                    break;
                case ETitleState.Validation:
                    ShowInputPanel();
                    break;
                case ETitleState.Valid:
                    ShowConfirmPanel();
                    break;
                case ETitleState.StartGame:
                    StartGame();
                    break;
            }
        }

        private void ShowInputPanel()
        {
            if (_nameInputPanel.IsPlayerNameEmpty())
            {
                _nameInputPanel.Show();
                return;
            }

            ShowConfirmPanel();
        }

        private void ShowConfirmPanel()
        {
            _inputConfirmPanel.Show();
            _nameInputPanel.Hide();

            _currentState = ETitleState.StartGame;
        }

        public void HideConfirmPanel()
        {
            _inputConfirmPanel.Hide();

            _nameInputPanel.SetPlayerName(string.Empty);
            _currentState = ETitleState.Validation;
            UpdateState();
        }


        public void StartGame()
        {
            _loadMapEvent.RequestLoad(_sceneToLoad);
        }
    }
}