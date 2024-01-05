using CryptoQuest.Sagas;
using IndiGames.Core.Events;

namespace CryptoQuest.UI.Popups.Sagas
{
    public class ServerMaintaining : ActionBase { }

    public class LoadTitleSceneOnMaintainSaga : SagaBase<ServerMaintaining>
    {
        protected override void HandleAction(ServerMaintaining ctx)
        {
            ActionDispatcher.Dispatch(new GoToTitleAction());
        }
    }
}