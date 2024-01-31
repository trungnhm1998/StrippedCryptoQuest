using System;
using CryptoQuest.Sagas.Objects;

namespace CryptoQuest.Ranch.Object
{
    [Serializable]
    public class UpgradeResponse
    {
        public int code;
        public bool success;
        public string message;
        public string uuid;
        public int gold;
        public float diamond;
        public int soul;
        public long time;
        public UpgradeResponseData data;

        [Serializable]
        public class UpgradeResponseData
        {
            public BeastResponseUpgradeData beast;
        }
    }
}