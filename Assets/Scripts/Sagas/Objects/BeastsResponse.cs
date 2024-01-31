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
        public float diamond;
        public int soul;
        public long time;
        public int page_size;
        public int page;
        public int total_page;
        public Data data;

        [Serializable]
        public class Data
        {
            public BeastResponse[] beasts;
        }
    }

    [Serializable]
    public class BeastResponse
    {
        public int id;
        public string beastTokenId;
        public string userId;
        public int level;
        public string beastId;
        public float HP;
        public float addHp;
        public float maxHP;
        public float MP;
        public float addMp;
        public float maxMP;
        public float strength;
        public float addStrength;
        public float vitality;
        public float addVitality;
        public float agility;
        public float addAgility;
        public float intelligence;
        public float addIntelligence;
        public float luck;
        public float addLuck;
        public float attack;
        public float addAttack;
        public float deffence;
        public float addDeffence;
        public float MATK;
        public float addMATK;
        public float evasionRate;
        public float criticalRate;
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
        public float minHP;
        public float minMP;
        public float minStrength;
        public float minVitality;
        public float minAgility;
        public float minIntelligence;
        public float minLuck;
        public float minAttack;
        public float minMATK;
        public float minDeffence;
        public float minEvasionRate;
        public float minCriticalRate;
        public int maxLv;
        public int minLv;
        public float randomNumberBonus;
        public float hpPerLv;
        public float mpPerLv;
        public float strengthPerLv;
        public float vitalityPerLv;
        public float agilityPerLv;
        public float intelligencePerLv;
        public float luckPerLv;
        public float maxStrength;
        public float maxVitality;
        public float maxAgility;
        public float maxIntelligence;
        public float maxLuck;
        public float maxAttack;
        public float maxMATK;
        public float maxDeffence;
        public float maxEvasionRate;
        public float maxCriticalRate;
        public int passiveSkillId;
        public int miningPower;
        public int maxFuel;
        public int consumeFuel;
        public string imageFileName;
        public string imageURL;
        public int equip;
        public int nft;
        public float maxHpAtMaxLv;
        public float maxMpAtMaxLv;
        public bool IsTransferring => transferring == 1;
        public bool IsEquipped => equip == 1;
    }

    
    [Serializable]
    public class BeastResponeEvolveData
    {
        public int transferring;
        public int equip;
        public int id;
        public string beastTokenId;
        public float HP;
        public float addHp;
        public float MP;
        public float addMp;
        public float strength;
        public float addStrength;
        public float vitality;
        public float addVitality;
        public float agility;
        public float addAgility;
        public float intelligence;
        public float addIntelligence;
        public float luck;
        public float addLuck;
        public int level;
        public string attachUnitTokenId;
        public int inGameStatus;
        public int mintStatus;
        public string userId;
        public string walletAddress;
        public string beastId;
        public float addAttack;
        public float addDefence;
        public float addMATK;
        public float evasionRate;
        public float criticalRate;
        public string createdAt;
        public string updatedAt;
    }

    [Serializable]
    public class BeastResponseUpgradeData
    {
        public int id;
        public string beastTokenId;
        public string userId;
        public string walletAddress;
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
        public int defence;
        public int addDefence;
        public int MATK;
        public int addMATK;
        public double evasionRate;
        public double criticalRate;
        public string attachUnitTokenId;
        public int inGameStatus;
        public int mintStatus;
        public int transferring;
        public int equip;
        public string createdAt;
        public string updatedAt;
    }
}