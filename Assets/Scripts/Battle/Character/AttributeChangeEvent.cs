using IndiGames.GameplayAbilitySystem.AttributeSystem;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Battle.Character
{
    public class AttributeChangeEvent : AttributesEventBase
    {
        public delegate void ChangeEvent(AttributeSystemBehaviour owner, AttributeValue oldValue,
            AttributeValue newValue);

        public event ChangeEvent Changed;
        [SerializeField] private AttributeScriptableObject _monitoringAttribute;

        public override void PreAttributeChange(AttributeSystemBehaviour attributeSystem,
            ref AttributeValue newAttributeValue) { }

        public override void PostAttributeChange(AttributeSystemBehaviour attributeSystem,
            ref AttributeValue oldAttributeValue,
            ref AttributeValue newAttributeValue)
        {
            if (oldAttributeValue.Attribute != _monitoringAttribute
                || newAttributeValue.Attribute != _monitoringAttribute) return;

            Changed?.Invoke(attributeSystem, oldAttributeValue, newAttributeValue);
        }
    }
}