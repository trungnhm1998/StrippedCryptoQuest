using System;
using CryptoQuest.Character.Attributes;
using UnityEngine;

namespace CryptoQuest.Gameplay.Inventory.Equipment
{
    [Serializable]
    public struct Stats
    {
        public int MaxLevel;
        public Attribute[] Attributes;
    }

    [Serializable]
    public struct Attribute
    {
        public AttributeScriptableObject AttributeDef;
        public float MinValue;
        public float MaxValue;
    }
}