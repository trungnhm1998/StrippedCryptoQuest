using System.Collections.Generic;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using UnityEditor;
using UnityEngine;

namespace IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Indigames Ability System/Attributes/Attribute")]
    public class AttributeScriptableObject : ScriptableObject
    {
        [SerializeField] private string _guid;
        public string Guid => _guid;

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            var assetPath = AssetDatabase.GetAssetPath(this);
            _guid = AssetDatabase.AssetPathToGUID(assetPath);
        }
#endif

        /// <summary>
        /// Called by <see cref="AttributeSystemBehaviour.InitializeAttributeValues"/> to calculate the initial value of the attribute.
        /// There would be a hidden bug here when the current attribute value depends on other attributes.
        /// </summary>
        /// <param name="attributeValue"></param>
        /// <param name="otherAttributeValues"></param>
        /// <returns></returns>
        public virtual AttributeValue CalculateInitialValue(AttributeValue attributeValue,
            List<AttributeValue> otherAttributeValues)
        {
            return attributeValue;
        }

        /// <summary>
        /// Called by <see cref="AttributeSystemBehaviour.UpdateAttributeValues"/> to calculate the current value of the attribute.
        /// return a new <see cref="AttributeValue"/> with the current value set.
        /// Wrap the base value with core modifier first <a herf="https://www.youtube.com/watch?v=JgSvuSaXs3E">source</a> before applying external modifier.
        /// </summary>
        /// <param name="attributeValue"></param>
        /// <param name="otherAttributeValuesInSystem"></param>
        /// <returns></returns>
        public virtual AttributeValue CalculateCurrentAttributeValue(AttributeValue attributeValue,
            List<AttributeValue> otherAttributeValuesInSystem)
        {
            if (attributeValue.ExternalModifier.Overriding != 0)
            {
                attributeValue.CurrentValue = attributeValue.ExternalModifier.Overriding;
                return attributeValue;
            }

            // order matters here, we want to override external with core
            if (attributeValue.CoreModifier.Overriding != 0)
            {
                attributeValue.CurrentValue = attributeValue.CoreModifier.Overriding;
                return attributeValue;
            }

            var coreValue = (attributeValue.BaseValue + attributeValue.CoreModifier.Additive) *
                            (attributeValue.CoreModifier.Multiplicative + 1);

            attributeValue.CurrentValue = (coreValue + attributeValue.ExternalModifier.Additive) *
                                          (attributeValue.ExternalModifier.Multiplicative + 1);
            return attributeValue;
        }
    }
}