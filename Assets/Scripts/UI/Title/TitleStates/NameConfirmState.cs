namespace CryptoQuest.UI.Title.TitleStates
{
    public class NameConfirmState : IState
    {
        private StartGamePanelController _startGamePanelController;
        private UINameConfirmPanel _confirmPanel;

        public NameConfirmState(StartGamePanelController startGamePanelController)
        {
            _startGamePanelController = startGamePanelController;
            _confirmPanel = startGamePanelController.UINameConfirmPanel;
        }

        public void OnEnter()
        {
            _confirmPanel.gameObject.SetActive(true);
            _confirmPanel.YesButton.Select();
            _confirmPanel.YesButton.onClick.AddListener(HandleYesClicked);
            _confirmPanel.NoButton.onClick.AddListener(HandleNoClicked);
        }


        public void OnExit()
        {
            _confirmPanel.gameObject.SetActive(false);
            _confirmPanel.YesButton.onClick.RemoveListener(HandleYesClicked);
            _confirmPanel.NoButton.onClick.RemoveListener(HandleNoClicked);
        }

        private void HandleYesClicked()
        {
            bool isInputValid = _startGamePanelController.UINamingPanel.IsInputValid();
            if (!isInputValid)
            {
                _startGamePanelController.ChangeState(new NameInputState(_startGamePanelController));
                return;
            }

            _confirmPanel.ConfirmPlayerName();
            _startGamePanelController.ChangeState(new PlayGameState(_startGamePanelController));
        }

        private void HandleNoClicked()
        {
            _startGamePanelController.ChangeState(new NameInputState(_startGamePanelController));
        }
    }
}