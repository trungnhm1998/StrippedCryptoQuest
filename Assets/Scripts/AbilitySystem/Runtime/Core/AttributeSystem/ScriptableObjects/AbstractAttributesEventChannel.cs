using System.Collections.Generic;
using UnityEngine;

namespace Indigames.AbilitySystem
{
    public abstract class AbstractAttributesEventChannel : ScriptableObject
    {
        /// <summary>
        /// Event raised after all the attribute changed 
        /// (like for clamp hp to max hp after the hp changed)
        /// </summary>
        /// <param name="attributeSystem"></param>
        /// <param name="previousAttributeValue"></param>
        /// <param name="currentAttributeValue"></param>
        public abstract void AfterAttributeChanged(
            AttributeSystemBehaviour attributeSystem,
            List<AttributeValue> previousAttributeValue,
            ref List<AttributeValue> currentAttributeValue
        );
    }
}