using CryptoQuest.Core;
using CryptoQuest.Networking.Actions;

namespace CryptoQuest.Sagas.Profile
{
    public class FetchEquipmentsOnAuthenticated : SagaBase<AuthenticateSucceed>
    {
        protected override void HandleAction(AuthenticateSucceed _) =>
            ActionDispatcher.Dispatch(new FetchProfileEquipmentsAction());
    }
}