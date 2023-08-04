using CryptoQuest.Input;
using IndiGames.Core.Events.ScriptableObjects;
using IndiGames.Core.SaveSystem;
using UnityEngine;

namespace CryptoQuest.UI.Title
{
    public class UITitleManager : MonoBehaviour
    {
        [SerializeField] private InputMediatorSO _inputMediator;
        [SerializeField] private SaveSystemSO _saveSystem;

        [Header("UI")]
        [SerializeField] private UIStartGame _startGamePanel;

        [SerializeField] private UINamingPanel _namingPanel;
        [SerializeField] private UINameConfirmPanel _confirmationPanel;

        [Header("Listen on")]
        [SerializeField] private VoidEventChannelSO _sceneLoadedChannel;

        [Header("Raise on")]
        [SerializeField] private VoidEventChannelSO _startNewGameChannel;

        [SerializeField] private VoidEventChannelSO _continueGameChannel;

        private bool _hasSaveData;

        private void Awake()
        {
            _hasSaveData = _saveSystem.LoadSaveGame();
        }

        private void Start()
        {
            _inputMediator.DisableAllInput();
            ShowStartGamePanel();
        }

        private void ShowStartGamePanel()
        {
            _startGamePanel.StartButtonPressed += HandleStartButtonPressed;
            _namingPanel.CancelEvent -= ShowStartGamePanel;

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
            _confirmationPanel.YesButtonPressed -= HandleNameConfirmed;
            _confirmationPanel.NoButtonPressed -= ShowNamingPanel;

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

            _confirmationPanel.YesButtonPressed += HandleNameConfirmed;
            _confirmationPanel.NoButtonPressed += ShowNamingPanel;

            _namingPanel.CancelEvent -= ShowStartGamePanel;
            _namingPanel.ConfirmNameButtonPressed -= ShowNameConfirmationPopup;
        }

        private void HandleNameConfirmed()
        {
            _confirmationPanel.YesButtonPressed -= HandleNameConfirmed;
            _confirmationPanel.NoButtonPressed -= ShowNamingPanel;

            _startNewGameChannel.RaiseEvent();
        }
    }
}