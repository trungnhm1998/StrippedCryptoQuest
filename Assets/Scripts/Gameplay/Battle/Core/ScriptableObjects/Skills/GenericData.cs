using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CryptoQuest
{
    [Serializable]
    public class GenericData : ScriptableObject
    {
        public int Id;
        public string NameKey = "";
        public string DescriptionKey = "";
    }
}