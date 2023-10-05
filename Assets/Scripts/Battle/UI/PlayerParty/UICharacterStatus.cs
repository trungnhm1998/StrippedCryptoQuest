using System.Collections.Generic;
using CryptoQuest.Character.Tag;
using IndiGames.GameplayAbilitySystem.TagSystem.ScriptableObjects;
using UnityEngine;
using UnityEngine.Pool;

namespace CryptoQuest.Battle.UI.PlayerParty
{
    [RequireComponent(typeof(UICharacterBattleInfo))]
    public class UICharacterStatus : MonoBehaviour
    {
        [SerializeField] private UICharacterBattleInfo _characterUI;
        [SerializeField] private Transform _statusContainer;
        [SerializeField] private UIStatusIcon _statusIconPrefab;

        private IObjectPool<UIStatusIcon> _statusIconPool;
        private Dictionary<TagSO, UIStatusIcon> _tagIconDict = new();

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
            _characterUI.Hero.AbilitySystem.TagSystem.TagAdded += TagAdded;
            _characterUI.Hero.AbilitySystem.TagSystem.TagRemoved += TagRemoved;
        }

        private void OnDisable()
        {
            _characterUI.Hero.AbilitySystem.TagSystem.TagAdded -= TagAdded;
            _characterUI.Hero.AbilitySystem.TagSystem.TagRemoved -= TagRemoved;
            ReleaseAllIcon();
        }

        private void TagAdded(params TagScriptableObject[] baseTags)
        {
            foreach (var baseTag in baseTags)
            {
                // TODO?: Is having a tag-icon mapping better than devired then reflection like this? 
                if (baseTag is not TagSO tagSO) continue;
                var statusIcon = _statusIconPool.Get();
                statusIcon.SetIcon(tagSO.Icon);
                _tagIconDict.Add(tagSO, statusIcon);
            }
        }

        private void TagRemoved(params TagScriptableObject[] baseTags)
        {
            foreach (var baseTag in baseTags)
            {
                if (baseTag is not TagSO tagSO) continue;
                if (!_tagIconDict.TryGetValue(tagSO, out var statusIcon)) continue;
                _tagIconDict.Remove(tagSO);
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
