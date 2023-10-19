using System.Collections;
using CryptoQuest.Battle.Events;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects;
using TinyMessenger;
using UnityEngine;

namespace CryptoQuest.Battle
{
    public class HandleRetreat : MonoBehaviour
    {
        [SerializeField] private BattleUnloader _unloader;
        [SerializeField] private BattleBus _bus;
        private TinyMessageSubscriptionToken _retreatedEvent;
        private TinyMessageSubscriptionToken _finishedPresentingEvent;

        private bool _canRetreat;

        private void Awake()
        {
            _finishedPresentingEvent = BattleEventBus.SubscribeEvent<FinishedPresentingEvent>(_ =>
            {
                if (_canRetreat) StartCoroutine(CoOnPresentEnd());
            });
            _retreatedEvent = BattleEventBus.SubscribeEvent<RetreatedEvent>(CacheState);
        }

        private void OnDestroy()
        {
            BattleEventBus.UnsubscribeEvent(_finishedPresentingEvent);
            BattleEventBus.UnsubscribeEvent(_retreatedEvent);
        }

        private void CacheState(RetreatedEvent ctx) => _canRetreat = true;

        private IEnumerator CoOnPresentEnd()
        {
            yield return _unloader.FadeInAndUnloadBattle();
            BattleEventBus.RaiseEvent(new BattleRetreatedEvent { Battlefield = _bus.CurrentBattlefield, });
        }
    }
}