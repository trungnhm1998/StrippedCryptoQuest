using System;
using CryptoQuest.Character.Attributes;
using UnityEngine.Serialization;

namespace CryptoQuest.Gameplay
{
    [Serializable]
    public struct StatsDef
    {
        public int MaxLevel;
        public CappedAttributeDef[] Attributes;
    }

    [Serializable]
    public struct CappedAttributeDef
    {
        [FormerlySerializedAs("AttributeDef")]
        public AttributeScriptableObject Attribute;
        public float MinValue;
        public float MaxValue;
    }
}