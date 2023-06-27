using System.Collections.Generic;
using UnityEngine;

namespace Indigames.AbilitySystem
{
    public abstract class AbstractAttributesEventChannel : ScriptableObject
    {
        /// <summary>
        /// I use old style of Observer pattern, because I need ref
        /// </summary>
        /// <param name="attributeSystem"></param>
        /// <param name="previousAttributeValue"></param>
        /// <param name="currentAttributeValue"></param>
        public abstract void PreAttributeChanged(
            AttributeSystemBehaviour attributeSystem,
            List<AttributeValue> previousAttributeValue,
            ref List<AttributeValue> currentAttributeValue
        );
    }
}