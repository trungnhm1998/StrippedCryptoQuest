using CryptoQuest.Inventory.Actions;
using IndiGames.Core.Events;

namespace CryptoQuest.BlackSmith.UpgradeStone.Sagas
{
    public class RemoveAfterUpgradeStoneSaga : SagaBase<RemoveAfterUpgradeStone>
    {
        protected override void HandleAction(RemoveAfterUpgradeStone ctx)
        {
            foreach (var stone in ctx.Stones)
            {
                ActionDispatcher.Dispatch(new RemoveStoneAction(stone));
            }
        }
    }
}