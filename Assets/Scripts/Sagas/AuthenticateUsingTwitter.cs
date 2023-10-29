using CryptoQuest.Core;
using CryptoQuest.Networking.Actions;

namespace CryptoQuest.Sagas
{
    public class AuthenticateUsingTwitter : AuthenticationSagaBase<LoginUsingTwitter>
    {
        // TODO: implement
        protected override void HandleAuthenticate(LoginUsingTwitter ctx)
        {
            ActionDispatcher.Dispatch(new DebugLoginAction());
        }
    }
}