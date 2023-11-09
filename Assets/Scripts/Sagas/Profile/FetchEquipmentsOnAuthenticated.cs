using CryptoQuest.Core;
using CryptoQuest.Networking.Actions;

namespace CryptoQuest.Sagas.Profile
{
    public class FetchEquipmentsOnAuthenticated : SagaBase<GetProfileSucceed>
    {
        protected override void HandleAction(GetProfileSucceed _) =>
            ActionDispatcher.Dispatch(new FetchProfileEquipmentsAction());
    }
}