using Core.Runtime.Events.ScriptableObjects;
using Core.Runtime.SaveSystem;
using Core.Runtime.SceneManagementSystem.Events.ScriptableObjects;
using Core.Runtime.SceneManagementSystem.ScriptableObjects;
using CryptoQuest.Input;
using CryptoQuest.System.Settings;
using TMPro;
using UnityEngine;
using NotImplementedException = System.NotImplementedException;

namespace CryptoQuest.UI
{
    public class UITitle : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _playerNamePlaceholder;
        [SerializeField] private TextMeshProUGUI _validationText;
        [SerializeField] private GameObject _panelInputName;
        public TextAsset _textAsset;

        [Space(10), SerializeField] private SceneScriptableObject _sceneToLoad;
        [SerializeField] private InputMediatorSO _inputMediatorSO;
        [SerializeField] private SaveSystemSO _saveSystemSo;

        [Header("Listen on")]
        [SerializeField] private VoidEventChannelSO _sceneLoaded;

        [Header("Raise on")]
        [SerializeField] private LoadSceneEventChannelSO _loadMapEvent;


        private void OnEnable()
        {
            _sceneLoaded.EventRaised += SceneLoadedEvent_Raised;
            _inputMediatorSO.MenuConfirmClicked += CheckPlayerName;
        }

        private void OnDisable()
        {
            _sceneLoaded.EventRaised -= SceneLoadedEvent_Raised;
            _inputMediatorSO.MenuConfirmClicked += CheckPlayerName;
        }

        private void SceneLoadedEvent_Raised()
        {
            _inputMediatorSO.EnableMenuInput();
        }

        private void CheckPlayerName()
        {
            if (IsPlayerNull())
            {
                ShowInputPanel();
                return;
            }

            StartGame();
        }

        private void ShowInputPanel()
        {
            if (!string.IsNullOrEmpty(_playerNamePlaceholder.text)) SetPlayerName();
            if (_panelInputName.activeSelf == true) return;
            _panelInputName.SetActive(true);
        }

        public void SetPlayerName()
        {
            bool isValid = WordsValidate(_playerNamePlaceholder.text);
            if (!isValid) return;

            _saveSystemSo.PlayerName = _playerNamePlaceholder.text;
            _panelInputName.SetActive(false);
            StartGame();
        }

        private bool WordsValidate(string text)
        {
            var isValid = TextValidation.ValidateWords(text, _textAsset);
            if (isValid)
            {
                _validationText.gameObject.SetActive(false);
            }

            _validationText.text = "Invalid name";
            _validationText.gameObject.SetActive(true);

            return isValid;
        }

        private void StartGame()
        {
            _loadMapEvent.RequestLoad(_sceneToLoad);
        }

        private bool IsPlayerNull()
        {
            return string.IsNullOrEmpty(_saveSystemSo.PlayerName);
        }
    }
}