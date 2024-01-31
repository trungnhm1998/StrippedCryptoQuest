using System;

namespace CryptoQuest.Sagas.Objects
{
    [Serializable]
    public class RewardResponse
    {
        public int code;
        public bool success;
        public string message;
        public string uuid;
        public int gold;
        public float diamond;
        public int soul;
        public long time;
        public Data data;
        public int page_size;
        public int page;
        public int total_page;

        [Serializable]
        public class Data
        {
            public int rewardGold;
            public int rewardMetad;
            public int rewardExp;
            public Rewards rewards;
        }

        [Serializable]
        public class Rewards
        {
            public Items[] items;
            public MagicStone[] stones;
            public EquipmentResponse[] equipments;
        }

        [Serializable]
        public class Items
        {
            public string itemId;
            public int itemNum;
        }
    }
}