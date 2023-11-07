using CryptoQuest.Networking.Actions;
using IndiGames.Firebase.Bridge;

namespace CryptoQuest.Sagas
{
    public class AuthenticateUsingFacebook : AuthenticationSagaBase<LoginUsingFacebook>
    {
        protected override void HandleAuthenticate(LoginUsingFacebook ctx)
        {
            FirebaseAuth.SignInWithFacebook(gameObject.name, nameof(OnUserSignedIn), nameof(OnUserSignedOut));
        }
    }
}