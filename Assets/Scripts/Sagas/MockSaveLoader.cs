using CryptoQuest.Actions;
using CryptoQuest.Core;
using CryptoQuest.System;

namespace CryptoQuest.Sagas
{
    public class MockSaveLoader : SagaBase<AuthenticateSucceed>
    {
        protected override void HandleAction(AuthenticateSucceed ctx)
        {
            var saveSystem = ServiceProvider.GetService<ISaveSystem>();
            saveSystem?.LoadGame();
            ActionDispatcher.Dispatch(new GetProfileSucceed());
        }
    }
}