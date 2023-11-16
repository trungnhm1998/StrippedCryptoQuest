using CryptoQuest.Battle.Components;
using CryptoQuest.Battle.Events;
using TinyMessenger;
using UnityEngine;

namespace CryptoQuest.Battle.VFX
{
    public class VFXOnTurnBehaviour : MonoBehaviour
    {
        [SerializeField] private VFXPresenter _vfxPresenter;
        private TinyMessageSubscriptionToken _turnStartedEvent;
        private TinyMessageSubscriptionToken _turnEndedEvent;

        private void OnEnable()
        {
            _turnStartedEvent = BattleEventBus.SubscribeEvent<TurnStartedEvent>(OnTurnStartedEvent);
            _turnEndedEvent = BattleEventBus.SubscribeEvent<TurnEndedEvent>(OnTurnEndedEvent);
        }

        private void OnDisable()
        {
            BattleEventBus.UnsubscribeEvent(_turnStartedEvent);
            BattleEventBus.UnsubscribeEvent(_turnEndedEvent);
        }

        private void OnTurnStartedEvent(TurnStartedEvent ctx)
        {
            bool isEnemy = ctx.Character.gameObject.CompareTag(EnemyBehaviour.Tag);
            _vfxPresenter.enabled = !isEnemy;
        }

        private void OnTurnEndedEvent(TurnEndedEvent _)
        {
            _vfxPresenter.enabled = true;
        }
    }
}