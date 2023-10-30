using CryptoQuest.Networking.Actions;
using IndiGames.Firebase.Bridge;
using UnityEngine;

namespace CryptoQuest.Sagas
{
    public class RegisterUsingEmailSaga : AuthenticationSagaBase<RegisterEmailAction>
    {
        protected override void HandleAuthenticate(RegisterEmailAction ctx)
        {
            Debug.Log("Registering using email");
            FirebaseAuth.CreateUserWithEmailAndPassword(ctx.Email, ctx.Password, name, nameof(OnUserSignedIn),
                nameof(OnUserSignedOut));
        }
    }
}