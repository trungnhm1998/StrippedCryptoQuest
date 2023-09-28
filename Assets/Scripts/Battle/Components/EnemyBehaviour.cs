using System;
using System.Collections;
using CryptoQuest.Character.Enemy;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.TagSystem.ScriptableObjects;
using Spine.Unity;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CryptoQuest.Battle.Components
{
    /// <summary>
    /// TODO: This name is too generic
    /// Simple stats provider which only return
    /// Attribute : Value
    /// </summary>
    public interface IStatsProvider
    {
        AttributeWithValue[] Stats { get; }
        void ProvideStats(AttributeWithValue[] attributeWithValues);
    }

    public class EnemyBehaviour : MonoBehaviour, IStatsProvider
    {
        public AttributeWithValue[] Stats => _enemyDef.Stats;

        public string DisplayName { get; private set; }
        private EnemySpec _spec = new();
        public EnemySpec Spec => _spec;
        private EnemyDef _enemyDef;
        private GameObject _enemyModel;

        private Character _battleCharacter;
        private SkeletonAnimation _skeletonAnimation;
        private AsyncOperationHandle<string> _localizedNameHandle;
        private AsyncOperationHandle<GameObject> _modelHandle;

        private void Awake()
        {
            _battleCharacter = GetComponent<Character>();
        }

        public void Init(EnemySpec enemySpec, string postfix)
        {
            // Data
            _spec = enemySpec;
            _enemyDef = _spec.Data;

            // Stats
            _battleCharacter.Init(_enemyDef.Element);

            // Visuals
            StartCoroutine(CoInstantiateModel());
            StartCoroutine(CoLoadName(postfix));
        }

        private IEnumerator CoInstantiateModel()
        {
            _modelHandle = _enemyDef.Model.InstantiateAsync(transform);
            yield return _modelHandle;
            _enemyModel = _modelHandle.Result;
            _skeletonAnimation = _enemyModel.GetComponentInChildren<SkeletonAnimation>();
        }

        private IEnumerator CoLoadName(string postfix)
        {
            if (_spec.Data.Name.IsEmpty)
            {
                Debug.LogWarning($"Localized string not set using default name {_spec.Data.Name}");
                DisplayName = $"{_spec.Data.Name}{postfix}";
                yield break;
            }

            _localizedNameHandle = _spec.Data.Name.GetLocalizedStringAsync();
            yield return _localizedNameHandle;
            var loadedSuccess = _localizedNameHandle.IsValid() && _localizedNameHandle.IsDone &&
                                _localizedNameHandle.Result != null &&
                                _localizedNameHandle.Status == AsyncOperationStatus.Succeeded;
            if (!loadedSuccess)
            {
                Debug.LogWarning($"Failed to load localized string for enemy using default name {_spec.Data.Name}");
                DisplayName = $"{_spec.Data.Name}{postfix}";
                yield break;
            }

            DisplayName = $"{_localizedNameHandle.Result}{postfix}";
        }

        public void SetAlpha(float alpha)
        {
            var color = _skeletonAnimation.Skeleton.GetColor();
            color.a = alpha;
            _skeletonAnimation.Skeleton.SetColor(color);
        }

        // TODO: Check enemy is alive
        public bool IsValid() => _spec.IsValid() && _enemyModel != null;

        private void OnDestroy()
        {
            if (_modelHandle.IsValid()) Addressables.Release(_modelHandle);
            if (_localizedNameHandle.IsValid()) Addressables.Release(_localizedNameHandle);
            if (_spec.IsValid() == false) return; // party not always full
            Destroy(_enemyModel);
        }

        public void ProvideStats(AttributeWithValue[] attributeWithValues) { }

        public bool HasTag(TagScriptableObject tagSO) => _battleCharacter.HasTag(tagSO);
    }
}