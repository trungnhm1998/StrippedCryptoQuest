using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Indigames.AbilitySystem
{
    [CreateAssetMenu(fileName = "InitStats", menuName = "GIndigames Ability System/Attribute/Initialize Stats Database")]
    public class InitializeAttributeDatabase : ScriptableObject
    {
        public AttributeInitValue[] attributesToInitialize;
    }

    [Serializable]
    public struct AttributeInitValue
    {
        public AttributeScriptableObject attribute;
        public float value;
    }
}