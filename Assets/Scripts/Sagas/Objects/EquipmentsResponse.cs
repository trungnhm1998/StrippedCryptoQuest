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
            public EquipmentResponse[] equipments;
        }
    }

    [Serializable]
    public class EquipmentResponse
    {
        public int id;
        public string equipmentTokenId;
        public string userId;
        public string walletAddress;
        public int lv;
        public string equipmentId;
        public int equipTypeId;
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
        public float intelligence;
        public float addIntelligence;
        public float luck;
        public float addLuck;
        public float attack;
        public float addAttack;
        public float MATK;
        public float addMATK;
        public float defence;
        public float addDefence;
        public float criticalRate;
        public float evasionRate;
        public string attachUnitTokenId;
        public int inGameStatus;
        public int mintStatus;
        public int transferring;
        public string createdAt;
        public string updatedAt;
        public int attachId;
        public string equipmentIdForeign;
        public string localizeKey;
        public string equipNameJp;
        public string equipTypeNameJp;
        public string seriesNameJp;
        public string equipName;
        public string equipTypeName;
        public string rarityName;
        public string categoryId;
        public string seriesName;
        public string equipPartId;
        public int rarityId;
        public string seriesId;
        public string groupId;
        public int star;
        public int evoLv;
        public int requiredLv;
        public int restrictedLv;
        public int price;
        public int sellingPrice;
        public int minLv;
        public int maxLv;
        public float minHP;
        public float minMP;
        public float minStrength;
        public float minVitality;
        public float minAgility;
        public float minIntelligence;
        public float minLuck;
        public float minAttack;
        public float minMATK;
        public float minDefence;
        public float minEvasionRate;
        public float minCriticalRate;
        public float maxHP;
        public float maxMP;
        public float maxStrength;
        public float maxVitality;
        public float maxAgility;
        public float maxIntelligence;
        public float maxLuck;
        public float maxAttack;
        public float maxMATK;
        public float maxDefence;
        public float maxEvasionRate;
        public float maxCriticalRate;
        public float miningPower;
        public float consumeFuel;
        public string imageFileName;
        public int passiveSkillId1;
        public int passiveSkillId2;
        public int conditionSkillId;
        public int nft;
        public int slot;
        public string imageURL;
        public int randomNumberBonus;
        public float valuePerLv;
        public MagicStone[] attachStones;
        public int[] passiveSkills;
        public int[] conditionSkills;
        public bool IsEquipped => !string.IsNullOrEmpty(attachUnitTokenId);
        public bool IsTransferrable => transferring == 0;
    }
}