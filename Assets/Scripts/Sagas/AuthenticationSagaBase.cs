using CryptoQuest.Core;
using CryptoQuest.Networking.Actions;
using UnityEngine;

namespace CryptoQuest.Sagas
{
    public abstract class AuthenticationSagaBase<TAction> : SagaBase<TAction> where TAction : ActionBase
    {
        protected void OnUserSignedIn(string json)
        {
            Debug.Log("FirebaseAuthScript: OnUserSignedIn");
            Debug.Log(json);
        }

        protected void OnUserSignedOut(string json)
        {
            Debug.Log("FirebaseAuthScript: OnUserSignedOut");
            Debug.Log(json);
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