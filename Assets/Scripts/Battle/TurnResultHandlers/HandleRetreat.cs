using System.Collections;
using CryptoQuest.Battle.Events;
using TinyMessenger;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.Battle.TurnResultHandlers
{
    public class HandleRetreat : MonoBehaviour
    {
        [SerializeField] private UnityEvent _onRetreated;
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

        private void CacheState(RetreatedEvent ctx)
        {
            _canRetreat = true;
            _onRetreated.Invoke();
        }

        private IEnumerator CoOnPresentEnd()
        {
            yield return _unloader.FadeInAndUnloadBattle();
            BattleEventBus.RaiseEvent(new BattleRetreatedEvent { Battlefield = _bus.CurrentBattlefield, });
        }
    }
}