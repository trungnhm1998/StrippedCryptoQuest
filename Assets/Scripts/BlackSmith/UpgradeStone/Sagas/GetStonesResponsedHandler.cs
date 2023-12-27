using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CryptoQuest.Item.MagicStone;
using CryptoQuest.Sagas.MagicStone;
using CryptoQuest.Sagas.Profile;
using IndiGames.Core.Common;
using IndiGames.Core.Events;

namespace CryptoQuest.BlackSmith.UpgradeStone.Sagas
{
    public class GetStonesResponsedHandler : SagaBase<GetStonesResponsed>
    {
        private IMagicStoneResponseConverter _converter;

        protected override void HandleAction(GetStonesResponsed ctx)
        {
            var stones = new List<IMagicStone>();

            _converter ??= ServiceProvider.GetService<IMagicStoneResponseConverter>();
            foreach (var data in ctx.Response.data.stones)
            {
                if (data.inGameStatus != 2) continue;
                var magicStone = _converter.Convert(data);
                stones.Add(magicStone);
            }

            StartCoroutine(CoLoadResponseData(stones));
            ActionDispatcher.Dispatch(new FetchProfileAction());
        }

        private IEnumerator CoLoadResponseData(List<IMagicStone> stones)
        {
            while (stones.Any(stone => stone.Passives.Length < 2))
            {
                yield return null;
            }

            ActionDispatcher.Dispatch(new ResponseGetMagicStonesSucceeded(stones));
        }
    }
}