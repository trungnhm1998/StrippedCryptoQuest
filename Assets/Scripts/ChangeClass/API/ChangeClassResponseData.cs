using System;
using Newtonsoft.Json;

namespace CryptoQuest.ChangeClass.API
{
    [Serializable]
    public class ChangeClassResponseData
    {
        public int code;
        public bool success;
        public string message;
        public string uuid;
        public int gold;
        public float diamond;
        public int soul;
        public long time;
        public UserMaterials data;
    }
    [Serializable]
    public class UserMaterials
    {
        public NewCharacter newCharacter;
        public UserMaterial userMaterials;
    }

    [Serializable]
    public class UserMaterial
    {
        public int id;
        public string userId;
        public string materialId;
        public int materialNum;
        public string createdAt;
        public string updatedAt;
        public string deletedAt;
    }

    [Serializable]
    public class NewCharacter
    {
        public int transferring;
        public int id;
        public string unitTokenId;
        public float HP;
        public float MP;
        public float addHp;
        public float addMp;
        public float maxHP;
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
        public float MATK;
        public float addMATK;
        public float attack;
        public float addAttack;
        public float deffence;
        public float addDeffence;
        public float evasionRate;
        public float criticalRate;
        public int level;
        public int exp;
        public int partyId;
        public int partyOrder;
        public int inGameStatus;
        public int isHero;
        public int itemAddedHP;
        public int mintStatus;
        public string userId;
        public string unitId;
        public string receivedAt;
        public string createdAt;
        public string updatedAt;
        public float maxHpAtMaxLv;
        public float maxMpAtMaxLv;
        public string name;
        [JsonProperty("class")]
        public string Class;
        public string element;
        public string personality;
        public string categoryId;
        public string characterId;
        public string classId;
        public string elementId;
        public string personalityId;
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
        public float maxFuel;
        public float consumeFuel;
        public string imageFileName;
        public string imageURL;
        public int isNFT;
        public object[] beasts;
        public object[] equipments;
    }
}