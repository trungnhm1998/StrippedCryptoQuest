using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Item.MagicStone;
using CryptoQuest.Sagas.MagicStone;
using CryptoQuest.UI.Actions;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
using UnityEngine;

namespace CryptoQuest.BlackSmith.UpgradeStone.Sagas
{
    public class UpgradeStonePreviewResponseHandler : SagaBase<UpgradeStonePreviewResponse>
    {
        private IMagicStoneResponseConverter _converter;

        protected override void HandleAction(UpgradeStonePreviewResponse ctx)
        {
            var responseData = ctx.Response;
            if (responseData.success)
            {
                CryptoQuest.Sagas.Objects.MagicStone data = Convert(responseData);
                _converter ??= ServiceProvider.GetService<IMagicStoneResponseConverter>();
                var magicStone = _converter.Convert(data);
                StartCoroutine(CoLoadResponseData(magicStone));
            }
        }

        private IEnumerator CoLoadResponseData(IMagicStone stone)
        {
            while (stone.Passives.Length < 2)
            {
                yield return null;
            }

            ActionDispatcher.Dispatch(new ShowLoading(false));
            ActionDispatcher.Dispatch(new UpgradePreviewSuccess(stone));
        }

        private CryptoQuest.Sagas.Objects.MagicStone Convert(StoneUpgradePreviewResponse response)
        {
            CryptoQuest.Sagas.Objects.MagicStone data = new()
            {
                id = 0,
                stoneLv = response.data.stoneLv,
                elementId = response.data.elementId,
                passiveSkillId1 = response.data.passiveSkillId1,
                passiveSkillId2 = response.data.passiveSkillId2
            };
            return data;
        }
    }
}