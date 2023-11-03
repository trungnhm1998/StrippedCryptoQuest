using System;

namespace CryptoQuest.Tavern.Objects
{
    [Serializable]
    public class CharactersResponse
    {
        public int code;
        public bool success;
        public string message;
        public string uuid;
        public int gold;
        public long time;
        public Data data;
    }

    [Serializable]
    public class Data
    {
        public Character[] characters;
    }

    public enum ETavernStatus
    {
        All = 0,
        InBox = 1,
        InGame = 2
    }

    [Serializable]
    public class Character
    {
        public int id;
        public string unitTokenId;
        public int inGameStatus;
        public string userId;
        public string unitId;
        public int HP;
        public int addHp;
        public int MP;
        public int addMp;
        public int maxHP;
        public int maxMP;
        public int strength;
        public int addStrength;
        public int vitality;
        public int addVitality;
        public int intelligence;
        public int addIntelligence;
        public int luck;
        public int addLuck;
        public int attack;
        public int addAttack;
        public int deffence;
        public int MATK;
        public int addMATK;
        public int addDeffence;
        public double criticalRate;
        public double evasionRate;
        public int level;
        public int exp;
        public int partyId;
        public int partyOrder;
        public int isHero;
        public int itemAddedHP;
        public string name;
        public string @class;
        public string element;
        public string personality;
        public string categoryId;
        public string characterId;
        public string classId;
        public string elementId;
        public string personalityId;
        public string imageFileName;
        public string imageURL;
        public int isNFT;
        public object[] beasts;
        public Equipment[] equipments;
    }

    [Serializable]
    public class Equipment
    {
        public int id;
        public string equipmentTokenId;
        public int inGameStatus;
        public int lv;
        public string equipmentId;
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
        public int MATK;
        public int addMATK;
        public int deffence;
        public int addDeffence;
        public int attack;
        public int addAttack;
        public string attachUnitTokenId;
        public string equipNameJp;
        public string descriptionJp;
        public string equipTypeNameJp;
        public string seriesNameJp;
        public string equipName;
        public string descriptionEn;
        public string equipTypeName;
        public string rarityName;
        public string categoryId;
        public string seriesName;
        public string equipPartId;
        public int equipTypeId;
        public int rarityId;
        public string seriesId;
        public string groupId;
        public int evoLv;
        public int restrictedLv;
        public int price;
        public int sellingPrice;
        public int evasionRate;
        public int criticalRate;
        public int maxLv;
        public int maxHP;
        public int maxMP;
        public int maxStrength;
        public int maxVitality;
        public int maxAgility;
        public int maxIntelligence;
        public int maxLuck;
        public int maxAttack;
        public int maxMATK;
        public int maxDeffence;
        public int maxEvasionRate;
        public int maxCriticalRate;
        public int miningPower;
        public int consumeFuel;
        public string imageFileName;
        public string imageURL;
        public int nft;
        public int star;
        public AttachStones attachStones;
        public object[] passiveSkills;
        public object[] conditionSkills;
        public int slot;
    }

    [Serializable]
    public class AttachStones { }
}