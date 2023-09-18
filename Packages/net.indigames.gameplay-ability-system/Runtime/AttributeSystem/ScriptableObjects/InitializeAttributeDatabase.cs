using System;
using UnityEngine;

namespace IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects
{
    [CreateAssetMenu(fileName = "InitStats",
        menuName = "Indigames Ability System/Attributes/Initialize Stats Database")]
    public class InitializeAttributeDatabase : ScriptableObject
    {
        public AttributeWithValue[] AttributesToInitialize;
    }

    [Serializable]
    public struct AttributeWithValue
    {
        public AttributeWithValue(AttributeScriptableObject attribute, float value)
        {
            Attribute = attribute;
            Value = value;
        }

        public AttributeScriptableObject Attribute;
        public float Value;
    }
}