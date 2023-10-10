using System.Collections.Generic;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.TagSystem.ScriptableObjects;
using UnityEngine;
using CoreAttribute = IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects.AttributeScriptableObject;

namespace CryptoQuest.Character.Attributes
{
    [CreateAssetMenu(menuName = "Crypto Quest/Character/Attribute Events/Max Cap Attribute AffectTag")]
    public class MaxCapAttributeAffectTag : AttributesEventBase
    {
        [SerializeField] private AttributeWithMaxCapped _attribute;
        [SerializeField] private TagScriptableObject _tagToAffect;

        public override void PreAttributeChange(AttributeSystemBehaviour attributeSystem,
            ref AttributeValue newAttributeValue) { }

        public override void PostAttributeChange(AttributeSystemBehaviour attributeSystem,
            ref AttributeValue oldAttributeValue,
            ref AttributeValue newAttributeValue)
        {
            if (_attribute != newAttributeValue.Attribute &&
                _attribute.CappedAttribute != newAttributeValue.Attribute) return;
            var cacheDict = attributeSystem.GetAttributeIndexCache();
            CheckApplyTag(attributeSystem, cacheDict, attributeSystem);
        }

        private void CheckApplyTag(AttributeSystemBehaviour attributeSystem,
            Dictionary<CoreAttribute, int> cacheDict, AttributeSystemBehaviour attributeSystemBehaviour)
        {
            if (!cacheDict.TryGetValue(_attribute, out int attributeIdx)
                || !cacheDict.TryGetValue(_attribute.CappedAttribute, out int maxAttributeIdx))
            {
                return;
            }

            var attributeValue = attributeSystemBehaviour.AttributeValues[attributeIdx];
            var maxAttributeValue = attributeSystemBehaviour.AttributeValues[maxAttributeIdx];

            var isMaxed = attributeValue.CurrentValue >= maxAttributeValue.CurrentValue;

            if (!attributeSystem.TryGetComponent<TagSystemBehaviour>(out var tagSystem)) return;

            if (isMaxed)
            {
                if (!tagSystem.HasTag(_tagToAffect)) tagSystem.AddTags(_tagToAffect);
                return;
            }

            tagSystem.RemoveTags(_tagToAffect);
        }
    }
}