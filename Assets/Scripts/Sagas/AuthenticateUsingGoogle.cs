using CryptoQuest.Networking.Actions;
using IndiGames.Firebase.Bridge;

namespace CryptoQuest.Sagas
{
    public class AuthenticateUsingGoogle : AuthenticationSagaBase<LoginUsingGoogle>
    {
        // TODO: implement
        protected override void HandleAuthenticate(LoginUsingGoogle ctx)
        {
            FirebaseAuth.SignInWithGoogle(gameObject.name, nameof(OnUserSignedIn), nameof(OnUserSignedOut));
        }
    }
}