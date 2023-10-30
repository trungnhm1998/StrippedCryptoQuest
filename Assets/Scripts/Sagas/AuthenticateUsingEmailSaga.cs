using CryptoQuest.Networking.Actions;
using IndiGames.Firebase.Bridge;

namespace CryptoQuest.Sagas
{
    public class AuthenticateUsingEmailSaga : AuthenticationSagaBase<AuthenticateUsingEmail>
    {
        protected override void HandleAuthenticate(AuthenticateUsingEmail ctx)
        {
            FirebaseAuth.SignInWithEmailAndPassword(ctx.Email, ctx.Password, name,
                nameof(OnUserSignedIn), nameof(OnUserSignedOut));
        }
    }
}