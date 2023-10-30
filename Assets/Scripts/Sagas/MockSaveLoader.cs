using CryptoQuest.Core;
using CryptoQuest.Networking.Actions;

namespace CryptoQuest.Sagas
{
    public class MockSaveLoader : SagaBase<AuthenticateSucceed>
    {
        protected override void HandleAction(AuthenticateSucceed ctx) =>
            ActionDispatcher.Dispatch(new GetProfileSucceed());
    }
}