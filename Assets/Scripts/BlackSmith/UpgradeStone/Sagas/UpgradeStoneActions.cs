using System.Collections.Generic;
using CryptoQuest.Item.MagicStone;
using IndiGames.Core.Events;

namespace CryptoQuest.BlackSmith.UpgradeStone.Sagas
{
    public class RequestUpgradeStone : ActionBase
    {
        public IMagicStone[] Stones;

        public RequestUpgradeStone(IMagicStone[] stoneIds)
        {
            Stones = stoneIds;
        }
    }

    public class UpgradeStoneResponsed : ActionBase
    {
        public StoneUpgradeResponse Response;

        public UpgradeStoneResponsed(StoneUpgradeResponse response)
        {
            Response = response;
        }
    }
    public class RemoveAfterUpgradeStone : ActionBase
    {
        public IMagicStone[] Stones;

        public RemoveAfterUpgradeStone(IMagicStone[] stoneIds)
        {
            Stones = stoneIds;
        }
    }

    public class RequestUpgradeStoneFailed : ActionBase { }

    public class ResponseUpgradeStoneFailed : ActionBase { }

    public class ResponseUpgradeStoneSuccess : ActionBase
    {
        public IMagicStone Stone;

        public ResponseUpgradeStoneSuccess(IMagicStone stone)
        {
            Stone = stone;
        }
    }

    public class UpgradeStonePreviewResponse : ActionBase
    {
        public StoneUpgradePreviewResponse Response;

        public UpgradeStonePreviewResponse(StoneUpgradePreviewResponse response)
        {
            Response = response;
        }
    }

    public class UpgradeStonePreviewRequest : ActionBase
    {
        public List<int> StoneIds;

        public UpgradeStonePreviewRequest(List<int> ids)
        {
            StoneIds = ids;
        }
    }

    public class UpgradePreviewSuccess : ActionBase
    {
        public IMagicStone Stone;

        public UpgradePreviewSuccess(IMagicStone stone)
        {
            Stone = stone;
        }
    }
}