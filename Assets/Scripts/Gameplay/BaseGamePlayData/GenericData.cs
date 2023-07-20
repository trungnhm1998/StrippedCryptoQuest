using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest
{
    [Serializable]
    public class GenericData : ScriptableObject
    {
        public int Id;
        public LocalizedString NameKey;
        public LocalizedString DescriptionKey;
    }
}