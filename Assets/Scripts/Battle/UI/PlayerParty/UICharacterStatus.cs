using System.Collections.Generic;
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

        private IObjectPool<UIStatusIcon> _statusIconPool;
        private Dictionary<TagScriptableObject, UIStatusIcon> _tagIconDict = new();
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
                if (_tagIconDict.TryGetValue(baseTag, out _)) continue;
                var statusIcon = _statusIconPool.Get();
                statusIcon.SetIcon(tagAsset.Icon);
                _tagIconDict.Add(baseTag, statusIcon);
            }
        }

        private void TagRemoved(params TagScriptableObject[] baseTags)
        {
            foreach (var baseTag in baseTags)
            {
                if (!TagAssetProvider.TryGetTagAsset(baseTag, out var tagAsset)) continue;
                if (!_tagIconDict.TryGetValue(baseTag, out var statusIcon)) continue;
                _tagIconDict.Remove(baseTag);
                statusIcon.ReleaseToPool();
            }
        }

        private void ReleaseAllIcon()
        {
            foreach (var icon in _tagIconDict.Values)
            {
                icon.ReleaseToPool();
            }

            _tagIconDict.Clear();
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