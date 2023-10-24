using System;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.TagSystem.ScriptableObjects;
using UnityEngine;
using CoreAttributeDef = IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects.AttributeScriptableObject;

namespace CryptoQuest.AbilitySystem.Attributes.Events
{
    [CreateAssetMenu(menuName = "Crypto Quest/Character/Attribute Events/Compare Attribute Value And Apply Tag")]
    public class ApplyTagIfConditionIsMet : AttributesEventBase
    {
        [Serializable]
        public class Condition
        {
            public enum ECompareType
            {
                GreaterThan = 0,
                LessThan = 1,
                EqualTo = 2,
                NotEqualTo = 3
            }

            [field: SerializeField] public TagScriptableObject TagToApply { get; private set; }
            [field: SerializeField] public CoreAttributeDef Attribute { get; private set; }
            [field: SerializeField] public ECompareType CompareType { get; private set; } = ECompareType.GreaterThan;
            [field: SerializeField] public float Value { get; private set; } = 1f;

            public bool IsMet(AttributeValue currentValue)
            {
                return CompareType switch
                {
                    ECompareType.GreaterThan => currentValue.CurrentValue > Value,
                    ECompareType.LessThan => currentValue.CurrentValue < Value,
                    ECompareType.EqualTo => currentValue.CurrentValue.NearlyEqual(Value),
                    ECompareType.NotEqualTo => !currentValue.CurrentValue.NearlyEqual(Value),
                    _ => false
                };
            }
        }

        public struct Context
        {
            public Battle.Components.Character Character;
            public TagScriptableObject TagToApply;
        }
        
        [SerializeField] private Condition _condition;

        public override void PostAttributeChange(AttributeSystemBehaviour attributeSystem,
            ref AttributeValue oldAttributeValue,
            ref AttributeValue newAttributeValue)
        {
            if (attributeSystem.TryGetComponent(out TagSystemBehaviour tagSystem) == false) return;
            if (_condition.Attribute != newAttributeValue.Attribute) return;
            if (_condition.IsMet(newAttributeValue) == false)
            {
                if (tagSystem.HasTag(_condition.TagToApply))
                {
                    tagSystem.RemoveTags(_condition.TagToApply);
                }
                return;
            }
            
            var ctx = new Context()
            {
                TagToApply = _condition.TagToApply,
                Character = attributeSystem.GetComponent<Battle.Components.Character>()
            };

            tagSystem.AddTags(_condition.TagToApply);
        }
        
        public override void PreAttributeChange(AttributeSystemBehaviour attributeSystem,
            ref AttributeValue newAttributeValue) { }
    }
}