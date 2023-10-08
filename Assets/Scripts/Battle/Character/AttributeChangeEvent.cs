using System;
using IndiGames.GameplayAbilitySystem.AttributeSystem;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Battle.Character
{
    public class AttributeChangeEvent : AttributesEventBase
    {
        public delegate void ChangeEvent(AttributeScriptableObject attribute, AttributeValue oldValue,
            AttributeValue newValue);
        public event ChangeEvent Changed;
        [SerializeField] private AttributeScriptableObject _monitoringAttribute;
        [SerializeField] private float _threshold = .001f;

        public override void PreAttributeChange(AttributeSystemBehaviour attributeSystem,
            ref AttributeValue newAttributeValue) { }

        public override void PostAttributeChange(AttributeSystemBehaviour attributeSystem,
            ref AttributeValue oldAttributeValue,
            ref AttributeValue newAttributeValue)
        {
            if (oldAttributeValue.Attribute != _monitoringAttribute
                || newAttributeValue.Attribute != _monitoringAttribute) return;

            if (newAttributeValue.CurrentValue.NearlyEqual(oldAttributeValue.CurrentValue, _threshold) == false) return;
            
            Changed?.Invoke(_monitoringAttribute, oldAttributeValue, newAttributeValue);
        }
    }
}