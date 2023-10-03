using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using IndiGames.GameplayAbilitySystem.TagSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Character.Attributes.ClampEventAction
{
    public class ApplyTagOnClamp : IClampAction
    {
        [SerializeField] private TagScriptableObject _tagToApply;

        public void OnClampSuccess(AttributeSystemBehaviour attributeSystem)
        {
            if (!attributeSystem.TryGetComponent<TagSystemBehaviour>(out var tagSystem)) return;
            if (!tagSystem.HasTag(_tagToApply)) tagSystem.AddTags(_tagToApply);
        }

        public void OnClampFailed(AttributeSystemBehaviour attributeSystem)
        {
            if (!attributeSystem.TryGetComponent<TagSystemBehaviour>(out var tagSystem)) return;
            tagSystem.RemoveTags(new TagScriptableObject[] {_tagToApply});
        }
    }
}