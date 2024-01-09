using CryptoQuest.Sagas;
using IndiGames.Core.Events;
using Proyecto26;

namespace CryptoQuest.UI.Popups.Sagas
{
    public class ServerMaintaining : ActionBase
    {
        public RequestException RequestException { get; }

        public ServerMaintaining(RequestException requestException)
        {
            RequestException = requestException;
        }
    }

    public class LoadTitleSceneOnMaintainSaga : SagaBase<ServerMaintaining>
    {
        protected override void HandleAction(ServerMaintaining ctx)
        {
            ActionDispatcher.Dispatch(new GoToTitleAction());
        }
    }
}