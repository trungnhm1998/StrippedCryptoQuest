namespace CryptoQuest.UI.Title.TitleStates
{
    public class WalletLoginState : SocialLoginState
    {
        public WalletLoginState(TitlePanelController titlePanelController) : base(titlePanelController) { }

        public override void OnEnter()
        {
            base.OnEnter();
            _socialPanel.RequestWalletLogin();
        }
    }
}