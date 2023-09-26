namespace CryptoQuest.UI.Title.TitleStates
{
    public class PlayGameState : IState
    {
        private StartGamePanelController _startGamePanelController;

        public PlayGameState(StartGamePanelController startGamePanelController)
        {
            _startGamePanelController = startGamePanelController;
        }

        public void OnEnter()
        {
            _startGamePanelController.StartGame();
        }

        public void OnExit() { }
    }
}