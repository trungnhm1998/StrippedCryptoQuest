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
            public float addDeffence;
            public float evasionRate;
            public float criticalRate;
            public float MATK;
            public float addMATK;
            public int level;
            public float exp;
            public int partyId;
            public int partyOrder;
            public int inGameStatus;
            public int isHero;
            public float itemAddedHP;
            public string userId;
            public string unitId;
            public string receivedAt;
            public string createdAt;
            public string updatedAt;
        }
    }
}