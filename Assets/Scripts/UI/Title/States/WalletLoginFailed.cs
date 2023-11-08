namespace CryptoQuest.UI.Title.States
{
    public class WalletLoginFailed : LoginFailedStateBase
    {
        protected override IState GetState() => new StartGameState();
    }
}