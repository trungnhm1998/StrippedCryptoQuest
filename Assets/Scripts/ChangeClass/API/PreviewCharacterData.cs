using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CryptoQuest.ChangeClass.API
{
    [Serializable]
    public class PreviewCharacterData
    {
        public int code;
        public bool success;
        public string message;
        public int gold;
        public int diamond;
        public int soul;
        public long time;
        public PreviewCharacter data;
    }

    [Serializable]
    public class PreviewCharacter
    {
        public float minHP;
        public float minMP;
        public float minAttack;
        public float minMATK;
        public float minDefence;
        public float minStrength;
        public float minVitality;
        public float minAgility;
        public float minIntelligence;
        public float minLuck;
    }
}
