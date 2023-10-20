using CryptoQuest.System;
using CryptoQuest.System.SaveSystem;

namespace CryptoQuest.UI.Title.TitleStates
{
    public class StartGameState : IState
    {
        private StartGamePanelController _startGamePanelController;
        private UIStartGame _startGamePanel;
        private UITitleSetting _titleSetting;

        public StartGameState(StartGamePanelController startGamePanelController)
        {
            _startGamePanelController = startGamePanelController;
            _startGamePanel = startGamePanelController.UIStartGame;
            _titleSetting = startGamePanelController.UITitleSetting;
        }

        public void OnEnter()
        {
            _startGamePanel.gameObject.SetActive(true);
            _startGamePanel.StartGameBtn.Select();
            _startGamePanel.StartGameBtn.onClick.AddListener(StartGameButtonPressed);
            _titleSetting.SettingButton.onClick.AddListener(SettingButtonPressed);
        }

        public void OnExit()
        {
            _startGamePanel.StartGameBtn.onClick.RemoveListener(StartGameButtonPressed);
            _titleSetting.SettingButton.onClick.RemoveListener(SettingButtonPressed);
            _startGamePanel.gameObject.SetActive(false);
        }

        private bool IsPlayerNameExist()
        {
            var saveSystem = ServiceProvider.GetService<ISaveSystem>();
            if (saveSystem != null)
            {
                saveSystem.LoadSaveGame();
                return !string.IsNullOrEmpty(saveSystem.PlayerName);
            }
            return false;
        }

        private void SettingButtonPressed()
        {
            _startGamePanelController.ChangeState(new GameSettingState(_startGamePanelController));
        }

        private void StartGameButtonPressed()
        {
            bool isPlayerNameExist = IsPlayerNameExist();
            HandleStartGame(isPlayerNameExist);
        }

        private void HandleStartGame(bool isPlayerNameExist)
        {
            if (!isPlayerNameExist)
            {
                _startGamePanelController.ChangeState(new NameInputState(_startGamePanelController));
                return;
            }

            _startGamePanelController.ChangeState(new PlayGameState(_startGamePanelController));
        }
    }
}