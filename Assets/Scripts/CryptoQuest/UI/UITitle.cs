using Core.Runtime.Events.ScriptableObjects;
using Core.Runtime.SaveSystem;
using Core.Runtime.SceneManagementSystem.Events.ScriptableObjects;
using Core.Runtime.SceneManagementSystem.ScriptableObjects;
using CryptoQuest.Input;
using CryptoQuest.System.Settings;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using NotImplementedException = System.NotImplementedException;

namespace CryptoQuest.UI
{
    public enum ETitleState
    {
        Waiting = 0,
        Validation = 1,
        StartGame = 2
    }

    public class UITitle : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _playerNamePlaceholder;
        [SerializeField] private LocalizeStringEvent _validationStringEvent;
        [SerializeField] private GameObject _panelInputName;
        [SerializeField] private TextAsset _textAsset;
        [SerializeField] private TextMeshProUGUI _validationText;

        [Space(10), SerializeField] private SceneScriptableObject _sceneToLoad;
        [SerializeField] private InputMediatorSO _inputMediatorSO;
        [SerializeField] private SaveSystemSO _saveSystemSo;

        [Header("Listen on")]
        [SerializeField] private VoidEventChannelSO _sceneLoaded;

        [Header("Raise on")]
        [SerializeField] private LoadSceneEventChannelSO _loadMapEvent;

        private IStringValidator _nameValidator;
        private ETitleState _currentState;

        private void Awake()
        {
            _nameValidator = new NameValidator(_textAsset);
        }

        private void OnEnable()
        {
            _sceneLoaded.EventRaised += SceneLoadedEvent_Raised;
            _inputMediatorSO.MenuConfirmClicked += MenuConfirmEvent_Clicked;
        }

        private void OnDisable()
        {
            _sceneLoaded.EventRaised -= SceneLoadedEvent_Raised;
            _inputMediatorSO.MenuConfirmClicked -= MenuConfirmEvent_Clicked;
        }

        private void SceneLoadedEvent_Raised()
        {
            _inputMediatorSO.EnableMenuInput();
        }

        private void MenuConfirmEvent_Clicked()
        {
            if (_currentState == ETitleState.Waiting)
            {
                _currentState = ETitleState.Validation;
                UpdateState();
                return;
            }

            if (_currentState == ETitleState.Validation)
            {
                ValidateInput();
                return;
            }
        }

        private void ValidateInput()
        {
            _currentState = ETitleState.StartGame;
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
                case ETitleState.StartGame:
                    StartGame();
                    break;
            }
        }

        private void ShowInputPanel()
        {
            if (IsPlayerNameEmpty())
            {
                _panelInputName.SetActive(true);
                return;
            }

            StartGame();
        }


        private void StartGame()
        {
            _loadMapEvent.RequestLoad(_sceneToLoad);
        }


        private bool IsPlayerNameEmpty()
        {
            return string.IsNullOrEmpty(_saveSystemSo.PlayerName);
        }
    }
}