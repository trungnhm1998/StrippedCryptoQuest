using System;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;

namespace CryptoQuest.Gameplay
{
    [Serializable]
    public class StatsDef
    {
        public int MaxLevel;
        public CappedAttributeDef[] Attributes = Array.Empty<CappedAttributeDef>();

        public bool IsValid() => Attributes.Length > 0;
    }

    [Serializable]
    public struct CappedAttributeDef
    {
        public AttributeScriptableObject Attribute;
        public float MinValue;
        public float MaxValue;
        public float RandomValue;
        public float ModifyValue;

        public CappedAttributeDef(AttributeScriptableObject attribute) : this()
        {
            Attribute = attribute;
        }
    }
}