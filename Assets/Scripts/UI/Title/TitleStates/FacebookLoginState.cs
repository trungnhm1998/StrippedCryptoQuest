namespace CryptoQuest.UI.Title.TitleStates
{
    public class FacebookLoginState : SocialLoginState
    {
        public FacebookLoginState(TitlePanelController titlePanelController) : base(titlePanelController) { }

        public override void OnEnter()
        {
            base.OnEnter();
            _socialPanel.RequestFacebookLogin();
        }
    }
}