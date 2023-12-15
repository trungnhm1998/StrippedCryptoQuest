using IndiGames.Core.Events;

namespace CryptoQuest.BlackSmith.UpgradeStone.Sagas
{
    public class RequestUpgradeStone : ActionBase
    {
        public int[] StoneIds;

        public RequestUpgradeStone(int[] stoneIds)
        {
            StoneIds = stoneIds;
        }
    }

    public class ResponseUpgradeStoneFailed : ActionBase { }

    public class ResponseUpgradeStoneSuccess : ActionBase { }
}