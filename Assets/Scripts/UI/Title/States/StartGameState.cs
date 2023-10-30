using CryptoQuest.Core;
using CryptoQuest.System;

namespace CryptoQuest.UI.Title.States
{
    public class StartGameState : IState
    {
        private UIStartGame _startGamePanel;
        private UITitleSetting _titleSetting;
        private TitleStateMachine _stateMachine;

        public void OnEnter(TitleStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            stateMachine.TryGetComponentInChildren(out _startGamePanel);
            stateMachine.TryGetComponentInChildren(out _titleSetting);
            _startGamePanel.gameObject.SetActive(true);
            _startGamePanel.StartGameBtn.Select();
            _startGamePanel.StartGameBtn.onClick.AddListener(StartGameButtonPressed);
            _titleSetting.SettingButton.onClick.AddListener(SettingButtonPressed);
        }

        public void OnExit(TitleStateMachine stateMachine)
        {
            _startGamePanel.StartGameBtn.onClick.RemoveListener(StartGameButtonPressed);
            _titleSetting.SettingButton.onClick.RemoveListener(SettingButtonPressed);
            _startGamePanel.gameObject.SetActive(false);
        }

        private void SettingButtonPressed()
        {
            _stateMachine.ChangeState(new SettingState());
        }

        private void StartGameButtonPressed()
        {
            if (!IsPlayerNameExist())
            {
                _stateMachine.ChangeState(new NameInputState());
                return;
            }

            ActionDispatcher.Dispatch(new StartGameAction());
        }

        private bool IsPlayerNameExist()
        {
            return false;
            // TODO: REFACTOR SAVE SYSTEM
            var saveSystem = ServiceProvider.GetService<ISaveSystem>();
            return !string.IsNullOrEmpty(saveSystem.PlayerName);
        }
    }
}