using CryptoQuest.Actions;
using IndiGames.Firebase.Bridge;

namespace CryptoQuest.Sagas
{
    public class AuthenticateUsingGoogle : AuthenticationSagaBase<LoginUsingGoogle>
    {
        protected override void HandleAuthenticate(LoginUsingGoogle ctx)
        {
            FirebaseAuth.SignInWithGoogle(gameObject.name, nameof(OnUserSignedIn), nameof(OnUserSignedOut));
        }
    }
}