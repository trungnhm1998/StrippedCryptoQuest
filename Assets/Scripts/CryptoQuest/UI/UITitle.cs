using Core.Runtime.Events.ScriptableObjects;
using Core.Runtime.SaveSystem;
using Core.Runtime.SceneManagementSystem.Events.ScriptableObjects;
using Core.Runtime.SceneManagementSystem.ScriptableObjects;
using CryptoQuest.Input;
using TMPro;
using UnityEngine;
using NotImplementedException = System.NotImplementedException;

namespace CryptoQuest.UI
{
    public class UITitle : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _playerNamePlaceholder;
        [SerializeField] private GameObject _panelInputName;

        [SerializeField] private SceneScriptableObject _sceneToLoad;
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
            _saveSystemSo.PlayerName = _playerNamePlaceholder.text;
            _panelInputName.SetActive(false);
            StartGame();
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