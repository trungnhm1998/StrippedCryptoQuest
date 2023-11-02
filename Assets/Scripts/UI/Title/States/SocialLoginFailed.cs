namespace CryptoQuest.UI.Title.States
{
    public class SocialLoginFailed : LoginFailedStateBase
    {
        protected override IState GetState() => new TitleState();
    }
}