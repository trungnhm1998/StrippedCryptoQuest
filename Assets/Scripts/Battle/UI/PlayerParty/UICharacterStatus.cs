using System.Collections.Generic;
using System.Linq;
using CryptoQuest.AbilitySystem;
using CryptoQuest.System;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.TagSystem.ScriptableObjects;
using UnityEngine;
using UnityEngine.Pool;

namespace CryptoQuest.Battle.UI.PlayerParty
{
    [RequireComponent(typeof(UICharacterBattleInfo))]
    public class UICharacterStatus : MonoBehaviour
    {
        private ITagAssetProvider _tagAssetProvider;

        private ITagAssetProvider TagAssetProvider =>
            _tagAssetProvider ??= ServiceProvider.GetService<ITagAssetProvider>();

        [SerializeField] private UICharacterBattleInfo _characterUI;
        [SerializeField] private Transform _statusContainer;
        [SerializeField] private UIStatusIcon _statusIconPrefab;
        [SerializeField] private TagScriptableObject[] _disallowedMultipleTags;

        private IObjectPool<UIStatusIcon> _statusIconPool;

        private struct Map
        {
            public TagScriptableObject Tag;
            public UIStatusIcon Icon;
        }

        private readonly List<Map> _statusIcons = new List<Map>();
        private TagSystemBehaviour CharacterTagSystem => _characterUI.Hero.AbilitySystem.TagSystem;

        private void OnValidate()
        {
            _characterUI = GetComponent<UICharacterBattleInfo>();
        }

        private void Awake()
        {
            _statusIconPool ??= new ObjectPool<UIStatusIcon>(OnCreate, OnGet, OnRelease, OnDestroyIcon);
        }

        private void OnEnable()
        {
            CharacterTagSystem.TagAdded += TagAdded;
            CharacterTagSystem.TagRemoved += TagRemoved;
        }

        private void OnDisable()
        {
            CharacterTagSystem.TagAdded -= TagAdded;
            CharacterTagSystem.TagRemoved -= TagRemoved;
            ReleaseAllIcon();
        }

        private void TagAdded(params TagScriptableObject[] baseTags)
        {
            foreach (var baseTag in baseTags)
            {
                if (!TagAssetProvider.TryGetTagAsset(baseTag, out var tagAsset)) continue;
                if (_disallowedMultipleTags.Contains(baseTag) && _statusIcons.Any(map => map.Tag == baseTag))
                    continue;
                var statusIcon = _statusIconPool.Get();
                statusIcon.SetIcon(tagAsset.Icon);
                _statusIcons.Add(new Map()
                {
                    Icon = statusIcon,
                    Tag = baseTag
                });
            }
        }

        private void TagRemoved(params TagScriptableObject[] baseTags)
        {
            foreach (var baseTag in baseTags)
            {
                for (int i = _statusIcons.Count - 1; i >= 0; i--)
                {
                    var map = _statusIcons[i];
                    if (map.Tag != baseTag) continue;
                    // prevent remove tag if it's disallowed multiple tag and there's another tag with same base tag
                    if (_disallowedMultipleTags.Contains(baseTag) && CharacterTagSystem.HasTag(baseTag)) continue;
                    map.Icon.ReleaseToPool();
                    _statusIcons.RemoveAt(i);
                    return;
                }
            }
        }

        private void ReleaseAllIcon()
        {
            foreach (var map in _statusIcons)
            {
                map.Icon.ReleaseToPool();
            }

            _statusIcons.Clear();
        }

        private void OnGet(UIStatusIcon icon)
        {
            icon.ObjectPool = _statusIconPool;
            icon.transform.SetAsLastSibling();
            icon.gameObject.SetActive(true);
        }

        private UIStatusIcon OnCreate()
            => Instantiate<UIStatusIcon>(_statusIconPrefab, _statusContainer);

        private void OnRelease(UIStatusIcon icon)
        {
            icon.gameObject.SetActive(false);
        }

        private void OnDestroyIcon(UIStatusIcon icon)
        {
            Destroy(icon.gameObject);
        }
    }
}