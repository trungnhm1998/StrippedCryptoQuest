using System;
using System.Collections.Generic;

namespace CryptoQuest.ChangeClass.API
{
    [Serializable]
    public class CharacterResponseData
    {
        public int code;
        public bool success;
        public string message;
        public int gold;
        public float diamond;
        public int soul;
        public long time;
        public CharacterData data;
    }

    [Serializable]
    public class CharacterData
    {
        public List<CharacterAPI> characters;
    }

    [Serializable]
    public class CharacterAPI
    {
        public int id;
        public string unitTokenId;
        public int inGameStatus;
        public int userId;
        public string unitId;
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
        public float criticalRate;
        public float evasionRate;
        public float minAgility;
        public int level;
        public float exp;
        public int partyId;
        public int partyOrder;
        public int isHero;
        public int itemAddedHP;
        public string name;
        public string Class;
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
        public object[] equipments;
    }
}
