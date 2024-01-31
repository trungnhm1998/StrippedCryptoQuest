using System;
using Newtonsoft.Json;

namespace CryptoQuest.Sagas.Objects
{
    public enum ECharacterStatus
    {
        All = 0,
        InBox = 1,
        InGame = 2
    }

    [Serializable]
    public class CharactersResponse
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

        [Serializable]
        public class Data
        {
            public Character[] characters;
        }
    }


    [Serializable]
    public class Character
    {
        public int id;
        public string userId;
        public string unitId;
        public string unitTokenId;
        public float HP;
        public float addHp;
        public float MP;
        public float addMp;
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
        public float attack;
        public float addAttack;
        public float deffence;
        public float addDeffence;
        public float luck;
        public float addLuck;
        public float MATK;
        public float addMATK;
        public float evasionRate;
        public float criticalRate;
        public int level;
        public int exp;
        public int partyId;
        public int partyOrder;
        public DateTime receivedAt;
        public int inGameStatus;
        public int isHero;
        public int itemAddedHP;
        public DateTime createdAt;
        public DateTime updatedAt;
        public int maxHpAtMaxLv;
        public int maxMpAtMaxLv;
        public string name;
        public string @class;
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
        public float miningPower;
        public int maxFuel;
        public int consumeFuel;
        public string imageFileName;
        public string imageURL;
        public int isNFT;
        public object[] beasts;
        [JsonProperty("modifyStats")]
        public ModifyStat[] modifyStats;
        public object[] equipments;
        public int transferring;
        public bool IsTransferring => transferring == 1;
    }

    [Serializable]
    public class ModifyStat
    {
        [JsonProperty("attribute")]
        public string AttributeName;
        [JsonProperty("value")]
        public float Value;
    }
}