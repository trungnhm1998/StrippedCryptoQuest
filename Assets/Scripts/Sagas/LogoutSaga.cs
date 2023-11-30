using IndiGames.Core.Events;
using UnityEngine;

namespace CryptoQuest.Sagas
{
    public class LogoutAction : ActionBase { }
    public class LogoutFinishedAction : ActionBase { }

    public class LogoutSaga : SagaBase<LogoutAction>
    {
        protected override void HandleAction(LogoutAction ctx)
        {
            PlayerPrefs.SetString(SNSAutoLoginSaga.SNS_SAVE_KEY, string.Empty);
            ActionDispatcher.Dispatch(new LogoutFinishedAction());
        }
    }
}