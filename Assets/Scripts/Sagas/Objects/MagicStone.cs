using System;

namespace CryptoQuest.Sagas.Objects
{
    public enum EMagicStoneStatus
    {
        All = 0,
        InBox = 1,
        InGame = 2
    }

    [Serializable]
    public class MagicStonesResponse : CommonResponse
    {
        public Data data;
        public int page_size;
        public int page;
        public int total_page;

        [Serializable]
        public class Data
        {
            public MagicStone[] stones;
        }
    }

    [Serializable]
    public class MagicStone
    {
        public int id;
        public string stoneTokenId;
        public string userId;
        public string stoneId;
        public int attachEquipment;
        public int inGameStatus;
        public int mintStatus;
        public int transferring;
        public string createdAt;
        public string updatedAt;
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