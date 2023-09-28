namespace CryptoQuest.UI.Title.TitleStates
{
    public class TwitterLoginState : SocialLoginState
    {
        public TwitterLoginState(TitlePanelController titlePanelController) : base(titlePanelController) { }

        public override void OnEnter()
        {
            base.OnEnter();
            _socialPanel.RequestTwitterLogin();
            _titlePanelController.ChangeState(new SocialLoginLoading());
        }
    }
}