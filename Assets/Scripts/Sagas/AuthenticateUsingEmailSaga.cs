using System;
using CryptoQuest.Core;
using CryptoQuest.Networking.Actions;
using IndiGames.Firebase.Bridge;
using Newtonsoft.Json;

namespace CryptoQuest.Sagas
{
    [Serializable]
    public class FailedResponse
    {
        public string code;
        public string name;
    }

    public class AuthenticateUsingEmailSaga : AuthenticationSagaBase<AuthenticateUsingEmail>
    {
        private AuthenticateUsingEmail _ctx;

        protected override void HandleAuthenticate(AuthenticateUsingEmail ctx)
        {
            _ctx = ctx;
            FirebaseAuth.SignInWithEmailAndPassword(ctx.Email, ctx.Password, name,
                nameof(OnUserSignedIn), nameof(RegisterThenLoginIfNew));
        }

        private void RegisterThenLoginIfNew(string res)
        {
            var response = JsonConvert.DeserializeObject<FailedResponse>(res);
            if (response.name != "FirebaseError") return;
            if (response.code != "auth/user-not-found") return;

            ActionDispatcher.Dispatch(new RegisterEmailAction(_ctx.Email, _ctx.Password));
        }
    }
}