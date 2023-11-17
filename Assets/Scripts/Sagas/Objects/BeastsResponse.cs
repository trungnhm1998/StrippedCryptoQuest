using System;

namespace CryptoQuest.Sagas.Objects
{
    public enum EBeastStatus
    {
        All = 0,
        InBox = 1,
        InGame = 2
    }

    [Serializable]
    public class BeastsResponse
    {
        public int code;
        public bool success;
        public string message;
        public string uuid;
        public int gold;
        public int diamond;
        public int soul;
        public long time;
        public int page_size;
        public int page;
        public int total_page;
        public Data data;

        [Serializable]
        public class Data
        {
            public Beast[] beasts;
        }
    }

    [Serializable]
    public class Beast
    {
        public int id;
        public string beastTokenId;
        public string userId;
        public int level;
        public string beastId;
        public int HP;
        public int addHp;
        public int maxHp;
        public int MP;
        public int addMp;
        public int maxMp;
        public int strength;
        public int addStrength;
        public int vitality;
        public int addVitality;
        public int agility;
        public int addAgility;
        public int intelligence;
        public int addIntelligence;
        public int luck;
        public int addLuck;
        public int attack;
        public int addAttack;
        public int deffence;
        public int addDeffence;
        public int MATK;
        public int addMATK;
        public double evasionRate;
        public double criticalRate;
        public string attachUnitTokenId;
        public int inGameStatus;
        public int mintStatus;
        public int transferring;
        public string createdAt;
        public string updatedAt;
        public int star;
        public string @class;
        public string name;
        public string elementName;
        public string beastType;
        public string classId;
        public string categoryId;
        public string characterId;
        public string elementId;
        public int beastTypeId;
        public int minHP;
        public int minMP;
        public int minStrength;
        public int minVitality;
        public int minAgility;
        public int minIntelligence;
        public int minLuck;
        public int minAttack;
        public int minMATK;
        public int minDeffence;
        public double minEvasionRate;
        public double minCriticalRate;
        public int maxLv;
        public int minLv;
        public int randomNumberBonus;
        public double hpPerLv;
        public double mpPerLv;
        public double strengthPerLv;
        public double vitalityPerLv;
        public double agilityPerLv;
        public double intelligencePerLv;
        public double luckPerLv;
        public int maxStrength;
        public int maxVitality;
        public int maxAgility;
        public int maxIntelligence;
        public int maxLuck;
        public int maxAttack;
        public int maxMATK;
        public int maxDeffence;
        public double maxEvasionRate;
        public double maxCriticalRate;
        public int passiveSkillId;
        public int miningPower;
        public int maxFuel;
        public int consumeFuel;
        public string imageFileName;
        public string imageURL;
        public int nft;
        public int maxHpAtMaxLv;
        public int maxMpAtMaxLv;
    }
}