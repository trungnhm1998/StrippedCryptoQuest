using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.TagSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Character.Attributes
{
    public class ReduceIncomingDamageWhenGuarded : AttributesEventBase
    {
        [SerializeField] private TagScriptableObject _guardTag = null;

        public override void PreAttributeChange(AttributeSystemBehaviour attributeSystem,
            ref AttributeValue newAttributeValue)
        {
            var tagSystem = attributeSystem.GetComponent<TagSystemBehaviour>();
            if (tagSystem == null)
            {
                Debug.LogError("TagSystemBehaviour not found on " + attributeSystem.name);
                return;
            }

            if (tagSystem.HasTag(_guardTag) == false) return;

            var attributeIndex = attributeSystem.GetAttributeIndexCache()[newAttributeValue.Attribute];
            var oldAttributeValue = attributeSystem.AttributeValues[attributeIndex];
            var damage = oldAttributeValue.CurrentValue - newAttributeValue.CurrentValue;
            var reducedDamage = damage / 2;

            Debug.Log($"Guard activated, reduce damage from: {damage} to {reducedDamage}");

            oldAttributeValue.BaseValue -= reducedDamage;
            newAttributeValue =
                oldAttributeValue.Attribute.CalculateCurrentAttributeValue(oldAttributeValue,
                    attributeSystem.AttributeValues);
        }

        public override void PostAttributeChange(AttributeSystemBehaviour attributeSystem,
            ref AttributeValue oldAttributeValue,
            ref AttributeValue newAttributeValue) { }
    }
}