using System;
using System.Collections.Generic;
using CryptoQuest.System;
using IndiGames.GameplayAbilitySystem.TagSystem.ScriptableObjects;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.AbilitySystem
{
    public interface ITagAssetProvider
    {
        public TagAsset GetTagAsset(TagScriptableObject tag);
        bool TryGetTagAsset(TagScriptableObject tagScriptableObject, out TagAsset tagAsset);
    }

    [Serializable]
    public struct TagAsset
    {
        public TagScriptableObject Tag;
        public Sprite Icon;
        public LocalizedString AddedMessage;
        public LocalizedString AffectMessage;
        public LocalizedString RemoveMessage;
    }

    public class TagAssetMapping : MonoBehaviour, ITagAssetProvider
    {
        [SerializeField] private TagAsset[] _tagAssets;

        private readonly Dictionary<TagScriptableObject, TagAsset> _cache = new();

        private void Awake()
        {
            foreach (var tagAsset in _tagAssets)
            {
                _cache.Add(tagAsset.Tag, tagAsset);
            }

            ServiceProvider.Provide<ITagAssetProvider>(this);
        }

        public TagAsset GetTagAsset(TagScriptableObject gameplayTag) => _cache[gameplayTag];
        public bool TryGetTagAsset(TagScriptableObject gameplayTag, out TagAsset tagAsset) =>
            _cache.TryGetValue(gameplayTag, out tagAsset);
    }
}