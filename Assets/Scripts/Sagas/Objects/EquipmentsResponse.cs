using System;

namespace CryptoQuest.Sagas.Objects
{
    public enum EEquipmentStatus
    {
        All = 0,
        InBox = 1,
        InGame = 2
    }

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
        public EquipmentResponse[] equipments;
    }

    [Serializable]
    public class EquipmentResponse
    {
        public uint id;
        public string equipmentTokenId;
        public int inGameStatus;
        public int lv;
        public string equipmentId;
        public float HP;
        public float addHp;
        public float maxHp;
        public float MP;
        public float addMp;
        public float maxMp;
        public float strength;
        public float addStrength;
        public float vitality;
        public float addVitality;
        public float agility;
        public float addAgility;
        public float floatelligence;
        public float addIntelligence;
        public float luck;
        public float addLuck;
        public float MATK;
        public float addMATK;
        public float deffence;
        public float addDeffence;
        public float attack;
        public float addAttack;
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
        public float evasionRate;
        public float criticalRate;
        public int maxLv;
        public float maxHP;
        public float maxMP;
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
        public float miningPower;
        public float consumeFuel;
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