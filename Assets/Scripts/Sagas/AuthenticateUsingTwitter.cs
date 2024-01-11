using CryptoQuest.Actions;
using IndiGames.Firebase.Bridge;
using UnityEngine;

namespace CryptoQuest.Sagas
{
    public class AuthenticateUsingTwitter : AuthenticationSagaBase<LoginUsingTwitter>
    {
        protected override void HandleAuthenticate(LoginUsingTwitter ctx)
        {
            FirebaseAuth.SignInWithTwitter(gameObject.name, nameof(OnUserSignedIn), nameof(OnUserSignedOut));
        }
    }
}