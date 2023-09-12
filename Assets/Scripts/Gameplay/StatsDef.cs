using System;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;

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
        public AttributeScriptableObject Attribute;
        public float MinValue;
        public float MaxValue;

        public CappedAttributeDef(AttributeScriptableObject attribute) : this()
        {
            Attribute = attribute;
        }
    }
}