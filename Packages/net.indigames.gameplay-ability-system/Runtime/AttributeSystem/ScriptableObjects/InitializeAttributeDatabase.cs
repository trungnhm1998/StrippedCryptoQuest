using System;
using UnityEngine;

namespace IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects
{
    [CreateAssetMenu(fileName = "InitStats", menuName = "Indigames Ability System/Attributes/Initialize Stats Database")]
    public class InitializeAttributeDatabase : ScriptableObject
    {
        public AttributeInitValue[] AttributesToInitialize;
    }

    [Serializable]
    public struct AttributeInitValue
    {
        public AttributeScriptableObject Attribute;
        public float Value;
    }
}