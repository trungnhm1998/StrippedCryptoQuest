using System.Collections.Generic;
using CryptoQuest.Item.MagicStone;
using CryptoQuest.Sagas.MagicStone;
using IndiGames.Core.Common;
using IndiGames.Core.Events;

namespace CryptoQuest.BlackSmith.UpgradeStone.Sagas
{
    public class GetStonesResponsedHandler : SagaBase<FetchIngameMagicStonesSuccess>
    {
        private IMagicStoneResponseConverter _converter;

        protected override void HandleAction(FetchIngameMagicStonesSuccess ctx)
        {
            var stones = new List<IMagicStone>();

            _converter ??= ServiceProvider.GetService<IMagicStoneResponseConverter>();
            foreach (var data in ctx.Stones)
            {
                if (data.inGameStatus != 2) continue;
                var magicStone = _converter.Convert(data);
                stones.Add(magicStone);
            }

            ActionDispatcher.Dispatch(new ResponseGetMagicStonesSucceeded(stones));
        }
    }
}