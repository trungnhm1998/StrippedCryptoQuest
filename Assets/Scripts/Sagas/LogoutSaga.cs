using CryptoQuest.Networking;
using IndiGames.Core.Events;
using UnityEngine;

namespace CryptoQuest.Sagas
{
    public class LogoutAction : ActionBase { }
    public class LogoutFinishedAction : ActionBase { }

    public class LogoutSaga : SagaBase<LogoutAction>
    {
        [SerializeField] private Credentials _credentials;

        protected override void HandleAction(LogoutAction ctx)
        {
            _credentials.Profile = new();
            PlayerPrefs.SetString(SNSAutoLoginSaga.SNS_SAVE_KEY, string.Empty);
            ActionDispatcher.Dispatch(new LogoutFinishedAction());
        }
    }
}