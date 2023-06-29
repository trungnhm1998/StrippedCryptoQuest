using System;
using UnityEngine;

namespace Indigames.AbilitySystem
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