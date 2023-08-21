using System.Collections.Generic;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using UnityEngine;

namespace IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects
{
    public abstract class AbstractAttributesEventChannel : ScriptableObject
    {
        /// <summary>
        /// This is called before the attribute value is changed.
        ///
        /// I usually clamp the value to a certain range or based on max attribute.
        /// </summary>
        /// <param name="attributeSystem"></param>
        /// <param name="previousAttributeValue"></param>
        /// <param name="currentAttributeValue"></param>
        public abstract void PreAttributeChange(
            AttributeSystemBehaviour attributeSystem,
            List<AttributeValue> previousAttributeValue,
            ref List<AttributeValue> currentAttributeValue
        );
    }
}