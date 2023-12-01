using System;
using CryptoQuest.Sagas.Objects;

namespace CryptoQuest.BlackSmith.Upgrade.Sagas
{
    [Serializable]
    public class UpgradeResponse
    {
        public int code;
        public bool success;
        public string message;
        public string uuid;
        public int gold;
        public int diamond;
        public int soul;
        public long time;
        public EquipmentResponseData data;

        [Serializable]
        public class EquipmentResponseData
        {
            public EquipmentResponse equipment;
        }
    }
}