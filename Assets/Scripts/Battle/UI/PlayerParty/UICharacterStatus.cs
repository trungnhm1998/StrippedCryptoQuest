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

        private IObjectPool<UIStatusIcon> _statusIconPool;

        private readonly Dictionary<TagScriptableObject, UIStatusIcon> _statusIcons = new();
        protected TagSystemBehaviour CharacterTagSystem => _characterUI.Hero.AbilitySystem.TagSystem;

        private void OnValidate()
        {
            _characterUI = GetComponent<UICharacterBattleInfo>();
        }

        private void Awake() =>
            _statusIconPool ??= new ObjectPool<UIStatusIcon>(OnCreate, OnGet, OnRelease, OnDestroyIcon);

        private void OnEnable()
        {
            CharacterTagSystem.TagAdded += TagAdded;
            CharacterTagSystem.TagRemoved += TagRemoved;
            ShowDeadTagAtStart();
        }

        private void OnDisable()
        {
            CharacterTagSystem.TagAdded -= TagAdded;
            CharacterTagSystem.TagRemoved -= TagRemoved;
            ReleaseAllIcon();
        }

        protected virtual void ShowDeadTagAtStart()
        {
            if (!CharacterTagSystem.HasTag(TagsDef.Dead)) return;
            
            TagAdded(TagsDef.Dead);
        }

        protected virtual void TagAdded(params TagScriptableObject[] baseTags)
        {
            foreach (var baseTag in baseTags)
            {
                if (_statusIcons.ContainsKey(baseTag)) continue;
                if (!TagAssetProvider.TryGetTagAsset(baseTag, out var tagAsset)) continue;
                var statusIcon = _statusIconPool.Get();
                statusIcon.SetIcon(tagAsset.Icon);
                _statusIcons.TryAdd(baseTag, statusIcon);
            }
        }

        protected virtual void TagRemoved(params TagScriptableObject[] baseTags)
        {
            foreach (var baseTag in baseTags)
            {
                // I remove check system HasTag here because this method is execute in present phase
                // so if the tag is removed and added again in the same round UI not being updated
                if (!_statusIcons.TryGetValue(baseTag, out var icon)) continue;
                icon.ReleaseToPool();
                _statusIcons.Remove(baseTag);
            }
        }

        private void ReleaseAllIcon()
        {
            foreach (var icon in _statusIcons.Values) icon.ReleaseToPool();
            _statusIcons.Clear();
        }

        private void OnGet(UIStatusIcon icon)
        {
            icon.ObjectPool = _statusIconPool;
            icon.transform.SetAsLastSibling();
            icon.gameObject.SetActive(true);
        }

        private UIStatusIcon OnCreate() => Instantiate(_statusIconPrefab, _statusContainer);

        private void OnRelease(UIStatusIcon icon) => icon.gameObject.SetActive(false);

        private void OnDestroyIcon(UIStatusIcon icon) => Destroy(icon.gameObject);
    }
}