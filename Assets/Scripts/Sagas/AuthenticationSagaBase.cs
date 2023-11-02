using CryptoQuest.Core;
using CryptoQuest.Networking.Actions;
using CryptoQuest.Sagas.Objects;
using Newtonsoft.Json;
using UnityEngine;

namespace CryptoQuest.Sagas
{
    public abstract class AuthenticationSagaBase<TAction> : SagaBase<TAction> where TAction : ActionBase
    {
        protected void OnUserSignedIn(string json)
        {
            Debug.Log("FirebaseAuthScript: OnUserSignedIn " + json);
            var firebaseUser = JsonConvert.DeserializeObject<FirebaseUser>(json);
            ActionDispatcher.Dispatch(new AuthenticateWithBackendAction()
                { Token = firebaseUser.stsTokenManager.accessToken });
        }

        protected void OnUserSignedOut(string json)
        {
            Debug.Log("FirebaseAuthScript: OnUserSignedOut " + json);
            ActionDispatcher.Dispatch(new AuthenticateFailed());
        }

        protected override void HandleAction(TAction ctx)
        {
#if UNITY_EDITOR
            ActionDispatcher.Dispatch(new DebugLoginAction());
#elif UNITY_WEBGL
            HandleAuthenticate(ctx);
#endif
        }

        /// <summary>
        /// This method only called if using WEBGL
        /// </summary>
        /// <param name="ctx"></param>
        protected abstract void HandleAuthenticate(TAction ctx);
    }
}