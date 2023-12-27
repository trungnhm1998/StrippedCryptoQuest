using System.Collections;
using CryptoQuest.Item.MagicStone;
using CryptoQuest.Sagas.MagicStone;
using CryptoQuest.Sagas.Profile;
using CryptoQuest.UI.Actions;
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
                StartCoroutine(CoLoadResponseData(magicStone));
                return;
            }

            ActionDispatcher.Dispatch(new ResponseUpgradeStoneFailed());
        }

        private IEnumerator CoLoadResponseData(IMagicStone stone)
        {
            while (stone.Passives.Length < 2)
            {
                yield return null;
            }

            ActionDispatcher.Dispatch(new ResponseUpgradeStoneSuccess(stone));
        }
    }
}