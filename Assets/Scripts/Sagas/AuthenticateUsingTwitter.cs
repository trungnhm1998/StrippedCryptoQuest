using System.Collections;
using CryptoQuest.Actions;
using CryptoQuest.UI.Actions;
using IndiGames.Core.Events;
using UnityEngine;

namespace CryptoQuest.Sagas
{
    public class AuthenticateUsingTwitter : AuthenticationSagaBase<LoginUsingTwitter>
    {
        // TODO: implement
        protected override void HandleAuthenticate(LoginUsingTwitter ctx)
        {
#if UNITY_EDITOR
            ActionDispatcher.Dispatch(new DebugLoginAction());
#endif
        }

        protected override void HandleAction(LoginUsingTwitter ctx)
        {
#if UNITY_EDITOR
            ActionDispatcher.Dispatch(new AuthenticateSucceed());
#else
            StartCoroutine(DelayLoginFailedCo());
#endif
        }

        private IEnumerator DelayLoginFailedCo()
        {
            yield return new WaitForSeconds(2);
            ActionDispatcher.Dispatch(new AuthenticateFailed());
            ActionDispatcher.Dispatch(new ShowLoading(false));
        }
    }
}