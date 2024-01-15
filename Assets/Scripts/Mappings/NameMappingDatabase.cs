using System;
using System.Collections.Generic;
using UnityEngine;

namespace CryptoQuest.Mappings
{
    public class NameMappingDatabase : ScriptableObject
    {
        public List<NameMapping> NameMappings;
    }

    [Serializable]
    public class NameMapping
    {
        public string Id;
        public string Name;
    }
}