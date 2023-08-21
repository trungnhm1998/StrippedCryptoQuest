using System;
using IndiGames.GameplayAbilitySystem.AttributeSystem;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using UnityEngine;
using System.Collections;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Events;
using DG.Tweening;
using Spine.Unity;

namespace CryptoQuest.UI.Battle.CharacterInfo
{
    public class MonsterInfo : CharacterInfoBase
    {
        // To hide spine object until it loaded and reset position
        private const float INIT_Z_OFFSET = 100f;
        [SerializeField] private float _waitTimeBeforeFadeOut = 1f;
        [SerializeField] private float _fadeOutDuration = 1f;
        [SerializeField] private float _blinkingInterval = 0.2f;
        public SkeletonAnimation SpineAnimation { get; set; }
        private Sequence _sequence;
        private string sequenceId = "FadeSkeleton";

        protected override void Setup()
        {
            //TODO: Refactor this later
            if (_characterInfo.Data is MonsterDataSO monsterData)
            {
                StartCoroutine(LoadMonsterPrefab(monsterData));
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _sequence?.Kill();
            DOTween.Kill(sequenceId);
        }

        private IEnumerator LoadMonsterPrefab(MonsterDataSO data)
        {
            var handle = data.MonsterPrefab.InstantiateAsync(Vector3.zero + Vector3.back * INIT_Z_OFFSET,
                Quaternion.identity, transform);
            yield return handle;
            if (!handle.IsDone) yield break;
            LoadPrefabComplete(handle.Result);
        }

        private void LoadPrefabComplete(GameObject monsterGO)
        {
            monsterGO.transform.localPosition = Vector3.zero;
            SpineAnimation = GetComponentInChildren<SkeletonAnimation>();
        }

        protected override void OnHPChanged(AttributeScriptableObject.AttributeEventArgs args)
        {
            if (args.System != _characterInfo.Owner.AttributeSystem) return;

            _characterInfo.Owner.AttributeSystem.TryGetAttributeValue(_hpAttributeSO, out AttributeValue hpValue);
            if (args.OldValue.CurrentValue < hpValue.CurrentValue) return;
            BlinkingSkeleton(() => { StartCoroutine(CoMonsterDeath(hpValue.CurrentValue)); });
        }

        private IEnumerator CoMonsterDeath(float monsterCurrentHp)
        {
            if (monsterCurrentHp <= 0)
            {
                yield return TriggerEnemyDeath();
            }
        }

        private IEnumerator TriggerEnemyDeath()
        {
            yield return new WaitForSeconds(_waitTimeBeforeFadeOut);
            FadeSkeleton(0f, _fadeOutDuration)
                .OnComplete(() => gameObject.SetActive(false));
        }

        private Tween FadeSkeleton(float alphaEndValue, float time)
        {
            return DOTween.To(() => SpineAnimation.skeleton.A,
                x => SpineAnimation.skeleton.A = x,
                alphaEndValue, time).SetId(sequenceId);
        }

        private void BlinkingSkeleton(Action action)
        {
            _sequence = DOTween.Sequence();
            _sequence
                .Append(FadeSkeleton(0, _blinkingInterval))
                .Append(FadeSkeleton(1, _blinkingInterval))
                .Append(FadeSkeleton(0, _blinkingInterval))
                .Append(FadeSkeleton(1, _blinkingInterval))
                .OnComplete(() => action?.Invoke());
        }

        protected override void OnSelected(string name) { }
        public override void SetSelectActive(bool value) { }
    }
}