using System;
using CryptoQuest.Sagas;
using CryptoQuest.Sagas.Objects;

namespace CryptoQuest.BlackSmith.UpgradeStone.Sagas
{
    [Serializable]
    public class StoneUpgradeResponse : CommonResponse
    {
        public StoneResponseData data;

        [Serializable]
        public class StoneResponseData
        {
            public int success;
            public MagicStone newStone;
        }
    }
}