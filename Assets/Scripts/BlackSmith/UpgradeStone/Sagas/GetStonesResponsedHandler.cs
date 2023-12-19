using System.Collections.Generic;
using CryptoQuest.Item.MagicStone;
using CryptoQuest.Sagas.MagicStone;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
using UnityEngine;

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

            ActionDispatcher.Dispatch(new ResponseGetMagicStonesSucceeded(stones));
        }
    }
}