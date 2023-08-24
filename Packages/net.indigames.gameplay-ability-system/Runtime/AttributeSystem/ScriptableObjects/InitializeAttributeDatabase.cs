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
        [field: SerializeField] public AttributeScriptableObject Attribute { get; set; }
        [field: SerializeField] public float Value { get; set; }
    }
}