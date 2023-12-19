using System;
using CryptoQuest.Sagas;
using CryptoQuest.Sagas.Objects;

namespace CryptoQuest.BlackSmith.Upgrade.Sagas
{
    [Serializable]
    public class UpgradeResponse : CommonResponse
    {
        public EquipmentResponseData data;

        [Serializable]
        public class EquipmentResponseData
        {
            public EquipmentResponse equipment;
        }
    }
}