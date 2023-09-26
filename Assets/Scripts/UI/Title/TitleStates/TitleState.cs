namespace CryptoQuest.UI.Title.TitleStates
{
    public class TitleState : IState
    {
        private TitlePanelController _titlePanelController;
        private UISocialPanel _socialPanel;

        public TitleState(TitlePanelController titlePanelController)
        {
            _titlePanelController = titlePanelController;
            _socialPanel = titlePanelController.SocialPanel;
        }

        public void OnEnter()
        {
            _socialPanel.FacebookLoginBtn.onClick.AddListener(ChangeFacebookLoginState);
            _socialPanel.TwitterLoginBtn.onClick.AddListener(ChangeTwitterLoginState);
            _socialPanel.GmailLoginBtn.onClick.AddListener(ChangeGmailLoginState);
            _socialPanel.WalletLoginBtn.onClick.AddListener(ChangeWalletLoginState);
            _titlePanelController.SelectDefault();
            _socialPanel.gameObject.SetActive(true);
        }

        public void OnExit()
        {
            _socialPanel.FacebookLoginBtn.onClick.RemoveListener(ChangeFacebookLoginState);
            _socialPanel.TwitterLoginBtn.onClick.RemoveListener(ChangeTwitterLoginState);
            _socialPanel.GmailLoginBtn.onClick.RemoveListener(ChangeGmailLoginState);
            _socialPanel.WalletLoginBtn.onClick.RemoveListener(ChangeWalletLoginState);
            _socialPanel.gameObject.SetActive(false);
        }

        private void ChangeFacebookLoginState()
        {
            _titlePanelController.ChangeState(new FacebookLoginState(_titlePanelController));
        }

        private void ChangeTwitterLoginState()
        {
            _titlePanelController.ChangeState(new TwitterLoginState(_titlePanelController));
        }

        private void ChangeGmailLoginState()
        {
            _titlePanelController.ChangeState(new GmailLoginState(_titlePanelController));
        }

        private void ChangeWalletLoginState()
        {
            _titlePanelController.ChangeState(new WalletLoginState(_titlePanelController));
        }
    }
}