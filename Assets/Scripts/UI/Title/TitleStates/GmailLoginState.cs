namespace CryptoQuest.UI.Title.TitleStates
{
    public class GmailLoginState : SocialLoginState
    {
        public GmailLoginState(TitlePanelController titlePanelController) : base(titlePanelController) { }

        public override void OnEnter()
        {
            base.OnEnter();
            _socialPanel.RequestGmailLogin();
            _titlePanelController.ChangeState(new SocialLoginLoading());
        }
    }
}