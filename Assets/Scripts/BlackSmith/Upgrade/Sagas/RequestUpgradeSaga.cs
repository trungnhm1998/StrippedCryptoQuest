using CryptoQuest.BlackSmith.Upgrade.Actions;
using CryptoQuest.Networking;
using CryptoQuest.UI.Actions;
using IndiGames.Core.Common;
using IndiGames.Core.Events;

namespace CryptoQuest.BlackSmith.Upgrade.Sagas
{
    public class RequestUpgradeSaga : SagaBase<RequestUpgrade>
    {
        protected override void HandleAction(RequestUpgrade ctx)
        {
            ActionDispatcher.Dispatch(new ShowLoading());
            var restClient = ServiceProvider.GetService<IRestClient>();
            // TODO: Subscribe and dispact UpgradeResponsed action here #2174
        }
    }
}