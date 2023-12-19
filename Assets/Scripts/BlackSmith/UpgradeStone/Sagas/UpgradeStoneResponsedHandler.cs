using CryptoQuest.Item.MagicStone;
using CryptoQuest.Sagas;
using CryptoQuest.Sagas.MagicStone;
using CryptoQuest.UI.Actions;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
using UnityEngine;

namespace CryptoQuest.BlackSmith.UpgradeStone.Sagas
{
    public class UpgradeStoneResponsedHandler : SagaBase<UpgradeStoneResponsed>
    {
        private IMagicStoneResponseConverter _converter;

        protected override void HandleAction(UpgradeStoneResponsed ctx)
        {
            var responseData = ctx.Response.data;
            ActionDispatcher.Dispatch(new UpdateWalletAction(ctx.Response));

            if (responseData.success == 1)
            {
                _converter ??= ServiceProvider.GetService<IMagicStoneResponseConverter>();
                var magicStone = _converter.Convert(responseData.newStone);

                ActionDispatcher.Dispatch(new ResponseUpgradeStoneSuccess(magicStone));
                return;
            }
            
            ActionDispatcher.Dispatch(new ResponseUpgradeStoneFailed());
        }
    }
}