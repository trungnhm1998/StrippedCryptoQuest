using CryptoQuest.Actions;
using IndiGames.Core.Events;

namespace CryptoQuest.Sagas.Profile
{
    public class FetchProfileOnAuthenticated : SagaBase<AuthenticateSucceed>
    {
        protected override void HandleAction(AuthenticateSucceed ctx) => ActionDispatcher.Dispatch(new FetchProfileAction());
    }
}