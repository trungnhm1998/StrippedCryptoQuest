using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace CryptoQuest.ChangeClass.API
{
    public class ChangeClassResponseData : MonoBehaviour
    {
        public int code;
        public bool success;
        public string message;
        public int gold;
        public int diamond;
        public int soul;
        public long time;
        public UserMaterials data;
    }
    [Serializable]
    public class UserMaterials
    {
        public NewCharacter newCharacter;
        public UserMaterial userMaterials;
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

        public class NewCharacter
        {
            public string id;
            public string unitTokenId;
            public int inGameStatus;
            public string userId;
            public string unitId;
            public float HP;
            public float addHP;
            public float MP;
            public float addMP;
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
            public float attack;
            public float addAttack;
            public float deffence;
            public float MATK;
            public float addMATK;
            public float addDeffence;
            public float evasionRate;
            public float criticalRate;
            public int level;
            public float exp;
            public int partyId;
            public int partyOrder;
            public int isHero;
            public float itemAddedHP;
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
}