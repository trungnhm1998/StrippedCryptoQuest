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
            _credentials.Reset();
            ActionDispatcher.Dispatch(new LogoutFinishedAction());
        }
    }
}