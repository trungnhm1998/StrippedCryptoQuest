using CryptoQuest.Actions;
using IndiGames.Core.Events;

namespace CryptoQuest.Sagas.Profile
{
    public class FetchInventoriesOnAuthenticated : SagaBase<GetProfileSucceed>
    {
        protected override void HandleAction(GetProfileSucceed _)
        {
            ActionDispatcher.Dispatch(new FetchProfileEquipmentsAction());
            ActionDispatcher.Dispatch(new FetchProfileCharactersAction());
            ActionDispatcher.Dispatch(new FetchProfileBeastAction());
        }
    }

}