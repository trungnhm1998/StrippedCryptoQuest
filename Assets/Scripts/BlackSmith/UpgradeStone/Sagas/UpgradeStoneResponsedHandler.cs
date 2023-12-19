using CryptoQuest.Sagas.MagicStone;
using CryptoQuest.Sagas.Profile;
using IndiGames.Core.Common;
using IndiGames.Core.Events;

namespace CryptoQuest.BlackSmith.UpgradeStone.Sagas
{
    public class UpgradeStoneResponsedHandler : SagaBase<UpgradeStoneResponsed>
    {
        private IMagicStoneResponseConverter _converter;

        protected override void HandleAction(UpgradeStoneResponsed ctx)
        {
            var responseData = ctx.Response.data;
            ActionDispatcher.Dispatch(new FetchProfileAction());

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