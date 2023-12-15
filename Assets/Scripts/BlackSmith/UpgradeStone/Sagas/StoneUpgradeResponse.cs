using System;
using CryptoQuest.Sagas.Objects;

namespace CryptoQuest.BlackSmith.UpgradeStone.Sagas
{
    [Serializable]
    public class StoneUpgradeResponse
    {
        public int code;
        public bool success;
        public string message;
        public string uuid;
        public int gold;
        public int diamond;
        public int soul;
        public long time;
        public StoneResponseData data;

        [Serializable]
        public class StoneResponseData
        {
            public int success;
            public MagicStone newStone;
        }
    }
}