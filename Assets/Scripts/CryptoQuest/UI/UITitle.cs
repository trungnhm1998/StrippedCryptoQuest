using Core.Runtime.Events.ScriptableObjects;
using Core.Runtime.SaveSystem;
using Core.Runtime.SceneManagementSystem.Events.ScriptableObjects;
using Core.Runtime.SceneManagementSystem.ScriptableObjects;
using CryptoQuest.Input;
using CryptoQuest.System.Settings;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;

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
        [SerializeField] private TMP_InputField _playerNamePlaceholder;
        [SerializeField] private TextMeshProUGUI _validationText;
        [SerializeField] private LocalizeStringEvent _validationStringEvent;
        [SerializeField] private GameObject _nameEntryPanel;
        [SerializeField] private GameObject _promptPanel;
        [SerializeField] private TextAsset _textAsset;

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
            _currentState = ETitleState.Waiting;
        }

        private void OnEnable()
        {
            _sceneLoaded.EventRaised += SceneLoadedEvent_Raised;
            _inputMediatorSO.MenuSubmitClicked += MenuSubmitEvent_Clicked;
            _inputMediatorSO.CancelEvent += CancelEvent_Clicked;
        }

        private void OnDisable()
        {
            _sceneLoaded.EventRaised -= SceneLoadedEvent_Raised;
            _inputMediatorSO.MenuSubmitClicked -= MenuSubmitEvent_Clicked;
            _inputMediatorSO.CancelEvent -= CancelEvent_Clicked;
        }


        private void CancelEvent_Clicked()
        {
            switch (_currentState)
            {
                case ETitleState.Validation:
                    _nameEntryPanel.SetActive(false);
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

            if (_currentState == ETitleState.StartGame)
            {
                StartGame();
            }
        }

        private void SceneLoadedEvent_Raised() => _inputMediatorSO.EnableMenuInput();

        private void ValidateInput()
        {
            if (!IsValid()) return;

            _currentState = ETitleState.Valid;
            UpdateState();
        }

        private bool IsValid()
        {
            var validation = _nameValidator.Validate(_playerNamePlaceholder.text);
            _validationStringEvent.StringReference.TableEntryReference = "TITLE_VALIDATE_" + (int)validation;

            if (validation != EValidation.Valid)
            {
                _validationText.color = Color.red;
                return false;
            }

            _saveSystemSo.PlayerName = _playerNamePlaceholder.text;
            _validationText.color = Color.white;
            return true;
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
            if (IsPlayerNameEmpty())
            {
                _nameEntryPanel.SetActive(true);
                return;
            }

            ShowConfirmPanel();
        }

        private void ShowConfirmPanel()
        {
            _promptPanel.SetActive(true);
            _nameEntryPanel.SetActive(false);

            _currentState = ETitleState.StartGame;
        }

        public void HideConfirmPanel()
        {
            _promptPanel.SetActive(false);

            _saveSystemSo.PlayerName = string.Empty;
            _currentState = ETitleState.Validation;
            UpdateState();
        }


        public void StartGame()
        {
            if (_currentState == ETitleState.StartGame) _loadMapEvent.RequestLoad(_sceneToLoad);
        }


        private bool IsPlayerNameEmpty()
        {
            return string.IsNullOrEmpty(_saveSystemSo.PlayerName);
        }
    }
}