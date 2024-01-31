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

    [Serializable]
    public class StoneUpgradePreviewResponse : CommonResponse
    {
        public StonePreviewResponseData data;

        [Serializable]
        public class StonePreviewResponseData
        {
            public int probability;
            public int gold;
            public float diamond;
            public string stoneId;
            public string stoneNameEn;
            public string stoneNameJp;
            public string element;
            public string elementId;
            public int stoneLv;
            public string afterUpgradeStoneId;
            public int skillType;
            public int passiveSkillId1;
            public int passiveSkillId2;
            public int price;
            public int sellingPrice;
        }
    }
}