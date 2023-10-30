using System.Collections;
using System.Linq;
using CryptoQuest.AbilitySystem;
using CryptoQuest.Battle.Events;
using CryptoQuest.Battle.Presenter.Commands;
using DG.Tweening;
using IndiGames.GameplayAbilitySystem.TagSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Battle.Components.EnemyComponents
{
    public class EnemyDeadCommand : IPresentCommand
    {
        private EnemyDeadBehaviour _enemyDeadBehaviour;

        public EnemyDeadCommand(EnemyDeadBehaviour behaviour)
        {
            _enemyDeadBehaviour = behaviour;
        }

        public IEnumerator Present()
        {
            yield return _enemyDeadBehaviour.FadeOut();
            yield break;
        }
    }

    public class EnemyDeadBehaviour : CharacterComponentBase
    {
        [SerializeField] private float _fadeOutDuration = 1.5f;
        [SerializeField] private Ease _fadeEase = Ease.InCubic;

        private EnemyBehaviour _enemyBehaviour;
        private readonly string _tweenId = "EnemyDeadFadeOut";

        public override void Init()
        {
            _enemyBehaviour = GetComponent<EnemyBehaviour>();
            _enemyBehaviour.AbilitySystem.TagSystem.TagAdded += CheckIfDeadTagAdded;
        }

        public IEnumerator FadeOut()
        {
            _enemyBehaviour.SetAlpha(1f);
            var tween = DOTween.To(() => _enemyBehaviour.Color.a, x => _enemyBehaviour.SetAlpha(x),
                0f, _fadeOutDuration).SetEase(_fadeEase).SetId(_tweenId);
            yield return tween.WaitForCompletion();
        }

        private void OnDestroy()
        {
            DOTween.Kill(_tweenId);
            if (!Character.IsValid()) return; 
            _enemyBehaviour.AbilitySystem.TagSystem.TagAdded -= CheckIfDeadTagAdded;
        }

        private void CheckIfDeadTagAdded(TagScriptableObject[] tagsAdded)
        {
            if (!tagsAdded.Contains(TagsDef.Dead)) return;
            var deadCommand = new EnemyDeadCommand(this);
            BattleEventBus.RaiseEvent(new EnqueuePresentCommandEvent(deadCommand));
        }
    }
}