using System.Collections;
using CryptoQuest.Actions;
using CryptoQuest.Core;
using CryptoQuest.UI.Actions;
using UnityEngine;

namespace CryptoQuest.Sagas
{
    public class AuthenticateUsingTwitter : AuthenticationSagaBase<LoginUsingTwitter>
    {
        // TODO: implement
        protected override void HandleAuthenticate(LoginUsingTwitter ctx)
        {
            ActionDispatcher.Dispatch(new DebugLoginAction());
        }

        protected override void HandleAction(LoginUsingTwitter ctx)
        {
            ActionDispatcher.Dispatch(new ShowLoading());
            StartCoroutine(DelayLoginFailedCo());
        }

        private IEnumerator DelayLoginFailedCo()
        {
            yield return new WaitForSeconds(2);
            ActionDispatcher.Dispatch(new AuthenticateFailed());
            ActionDispatcher.Dispatch(new ShowLoading(false));
        }
    }
}