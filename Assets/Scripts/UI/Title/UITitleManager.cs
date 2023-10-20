using CryptoQuest.System.SaveSystem;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.UI.Title
{
    public class UITitleManager : MonoBehaviour
    {
        [SerializeField] private SaveSystemSO _saveSystem;

        [Header("UI")]
        [SerializeField] private UIStartGame _startGamePanel;

        [SerializeField] private UITitleSetting _titleSetting;
        [SerializeField] private UINamingPanel _namingPanel;
        [SerializeField] private UINameConfirmPanel _confirmationPanel;
        [SerializeField] private UISocialPanel socialPanel;
        [SerializeField] private UISignInPanel _signInPanel;
        [SerializeField] private UIOptionPanel _optionPanel;

        [Header("Listen on")]
        [SerializeField] private VoidEventChannelSO _sceneLoadedChannel;

        [SerializeField] private VoidEventChannelSO _loginSuccessEventChannel;

        [Header("Raise on")]
        [SerializeField] private VoidEventChannelSO _startNewGameChannel;

        [SerializeField] private VoidEventChannelSO _continueGameChannel;


        private bool _hasSaveData;

        private void OnEnable()
        {
            _loginSuccessEventChannel.EventRaised += HandleLoginSuccess;
        }

        private void OnDisable()
        {
            _loginSuccessEventChannel.EventRaised -= HandleLoginSuccess;
        }

        private void Start()
        {
            ShowSocialButtonPanel();
            _hasSaveData = _saveSystem?.PlayerName != null;
        }

        private void ShowSocialButtonPanel()
        {
            socialPanel.gameObject.SetActive(true);
            _confirmationPanel.gameObject.SetActive(false);
            _namingPanel.gameObject.SetActive(false);
            _startGamePanel.gameObject.SetActive(false);
        }

        private void ShowStartGamePanel()
        {
            _startGamePanel.InitStartGameUI();
            _startGamePanel.StartButtonPressed += HandleStartButtonPressed;
            _namingPanel.CancelEvent -= ShowStartGamePanel;

            socialPanel.gameObject.SetActive(false);
            _signInPanel.gameObject.SetActive(false);
            _confirmationPanel.gameObject.SetActive(false);
            _namingPanel.gameObject.SetActive(false);
            _startGamePanel.gameObject.SetActive(true);
        }

        private void HandleStartButtonPressed()
        {
            _startGamePanel.StartButtonPressed -= HandleStartButtonPressed;

            if (_hasSaveData)
                _continueGameChannel.EventRaised();
            else
                ShowNamingPanel();
        }

        private void ShowNamingPanel()
        {
            _startGamePanel.gameObject.SetActive(false);
            _confirmationPanel.gameObject.SetActive(false);
            _namingPanel.gameObject.SetActive(true);

            _namingPanel.ConfirmNameButtonPressed += ShowNameConfirmationPopup;
            _namingPanel.CancelEvent += ShowStartGamePanel;
        }

        private void ShowNameConfirmationPopup()
        {
            _namingPanel.gameObject.SetActive(false);
            _namingPanel.gameObject.SetActive(false);
            _confirmationPanel.gameObject.SetActive(true);

            _namingPanel.CancelEvent -= ShowStartGamePanel;
            _namingPanel.ConfirmNameButtonPressed -= ShowNameConfirmationPopup;
        }

        private void HandleLoginSuccess()
        {
            ShowStartGamePanel();
        }
    }
}