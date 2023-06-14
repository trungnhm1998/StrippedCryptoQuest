using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CryptoQuest
{
    public class CharacterSO : ScriptableObject
    {
        public int id;
        public string displayName;
        public bool isMaleGender;
        public int age;
        public string birthplace;
        public string height;
        public string weight;
        public string description;
    }
}
