using System;
using System.Collections.Generic;
using IndiGames.GameplayAbilitySystem.AttributeSystem;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.AbilitySystem.Attributes
{
    /// <summary>
    /// For                 Attack,     Skill Attack,       Defense,        Evade Rate,         Critical Hit Rate
    /// Affected by         STR,        INT,                VIT,            AGI,                LUK
    ///
    /// Equip attribute
    /// </summary>
    [CreateAssetMenu(menuName = "Crypto Quest/Character/Derived Attribute")]
    public class DerivedAttributeScriptableObject : AttributeScriptableObject
    {
        [SerializeField] private AttributeScriptableObject _affectedBy;
        [SerializeField] private float _value = 1;

        public override AttributeValue CalculateCurrentAttributeValue(AttributeValue attributeValue,
            List<AttributeValue> otherAttributeValuesInSystem)
        {
            var affectAttributeValue = otherAttributeValuesInSystem.Find(x => x.Attribute == _affectedBy);

            attributeValue.BaseValue = MathF.Pow(affectAttributeValue.CurrentValue, _value);
            var value = base.CalculateCurrentAttributeValue(attributeValue, otherAttributeValuesInSystem);
            return value;
        }
    }
}