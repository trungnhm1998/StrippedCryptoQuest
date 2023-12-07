using CryptoQuest.Sagas.MagicStone;
using IndiGames.Core.Events;

namespace CryptoQuest.Menus.DimensionalBox.Sagas
{
    public class NotifyFetchNftMagicStoneSucceed : SagaBase<FetchNftMagicStonesSucceed>
    {
        protected override void HandleAction(FetchNftMagicStonesSucceed ctx)
        {
            ActionDispatcher.Dispatch(new FetchProfileMagicStonesAction());
        }
    }
}