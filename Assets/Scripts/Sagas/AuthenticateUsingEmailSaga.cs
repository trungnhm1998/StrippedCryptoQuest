using CryptoQuest.Networking.Actions;
using IndiGames.Firebase.Bridge;
using UnityEngine;

namespace CryptoQuest.Sagas
{
    public class AuthenticateUsingEmailSaga : AuthenticationSagaBase<AuthenticateUsingEmail>
    {
        // TODO: implement
        protected override void HandleAuthenticate(AuthenticateUsingEmail ctx)
        {
            Debug.Log("FirebaseAuthScript: Sign in with email and password no param");
            FirebaseAuth.SignInWithEmailAndPassword(ctx.Email, ctx.Password, gameObject.name,
                nameof(OnUserSignedIn), nameof(OnUserSignedOut));
        }
    }
}