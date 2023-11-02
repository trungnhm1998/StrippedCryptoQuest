namespace CryptoQuest.UI.Title.States
{
    public class MailLoginFailed : LoginFailedStateBase
    {
        protected override IState GetState() => new MailLoginState();
    }
}