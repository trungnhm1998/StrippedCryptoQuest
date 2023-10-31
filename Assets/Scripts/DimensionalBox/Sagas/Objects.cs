using System;

namespace CryptoQuest.DimensionalBox.Sagas
{
    [Serializable]
    public class EquipmentsResponse
    {
        public int code;
        public bool success;
        public string message;
        public string uuid;
        public int gold;
        public int diamond;
        public int soul;
        public long time;
        public Data data;
        public int page_size;
        public int page;
        public int total_page;
    }

    [Serializable]
    public class Data
    {
        public Equipments[] equipments;
    }
    
    public enum EDimensionalBoxStatus
    {
        InBox = 1,
        InGame = 2
    }

    [Serializable]
    public class Equipments
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
        public string equipmentIdForeign;
    }

    [Serializable]
    public class AttachStones { }
}