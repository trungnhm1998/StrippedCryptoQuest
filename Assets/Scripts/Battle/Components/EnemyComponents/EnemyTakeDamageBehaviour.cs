using System.Collections;
using CryptoQuest.AbilitySystem.Attributes;
using CryptoQuest.AbilitySystem.Executions;
using CryptoQuest.Battle.Events;
using CryptoQuest.Battle.Presenter.Commands;
using DG.Tweening;
using TinyMessenger;
using UnityEngine;

namespace CryptoQuest.Battle.Components.EnemyComponents
{
    public class EnemyTakeDamageCommand : IPresentCommand
    {
        private EnemyTakeDamageBehaviour _enemyTakeDamageBehaviour;

        public EnemyTakeDamageCommand(EnemyTakeDamageBehaviour behaviour)
        {
            _enemyTakeDamageBehaviour = behaviour;
        }

        public IEnumerator Present()
        {
            yield return _enemyTakeDamageBehaviour.Blink();
            yield break;
        }
    }

    public class EnemyTakeDamageBehaviour : CharacterComponentBase
    {
        [SerializeField] private float _fadeDuration = 0.5f;
        [SerializeField] private float _blinkDelay = 0.5f;
        [SerializeField] private int _blinkLoopTimes = 2;
        [SerializeField] private Ease _fadeEase = Ease.InCubic;

        private TinyMessageSubscriptionToken _logDealtDamageEvent;

        private EnemyBehaviour _enemyBehaviour;
        private Sequence _sequence;

        public IEnumerator Blink()
        {
            _enemyBehaviour.SetAlpha(1f);
            _sequence = DOTween.Sequence();
            _sequence.Append(TweenEnemyAlpha(0.5f))
                .Append(TweenEnemyAlpha(1f))
                .SetDelay(_blinkDelay)
                .SetLoops(_blinkLoopTimes);
            
            yield return _sequence.WaitForCompletion();
        }

        private Tween TweenEnemyAlpha(float toAlpha)
        {
            return DOTween.To(() => _enemyBehaviour.Color.a, x => _enemyBehaviour.SetAlpha(x),
                toAlpha, _fadeDuration).SetEase(_fadeEase);
        }

        public override void Init()
        {
            _enemyBehaviour = GetComponent<EnemyBehaviour>();
        }

        private void OnEnable()
        {
            // I listen to log dealth damage event instead of damage event
            // because I want this behaviour to perform before log
            _logDealtDamageEvent = BattleEventBus.SubscribeEvent<LogDealtDamageEvent>(
                CreateTakeDamagePresent);
        }

        private void OnDestroy()
        {
            DOTween.Kill(_sequence);
            BattleEventBus.UnsubscribeEvent(_logDealtDamageEvent);
        }

        private void CreateTakeDamagePresent(LogDealtDamageEvent ctx)
        {
            if (ctx.Character != _enemyBehaviour) return;

            var takeDamageCommand = new EnemyTakeDamageCommand(this);
            BattleEventBus.RaiseEvent<EnqueuePresentCommandEvent>(
                new EnqueuePresentCommandEvent(takeDamageCommand));
        }
    }
}