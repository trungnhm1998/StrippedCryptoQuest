using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using UnityEngine;

namespace IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects
{
    public abstract class AbstractAttributesEventChannel : ScriptableObject
    {
        public abstract void PreAttributeChange(
            AttributeSystemBehaviour attributeSystem,
            ref AttributeValue newAttributeValue
        );

        public abstract void PostAttributeChange(
            AttributeSystemBehaviour attributeSystem,
            ref AttributeValue oldAttributeValue,
            ref AttributeValue newAttributeValue
        );
    }
}