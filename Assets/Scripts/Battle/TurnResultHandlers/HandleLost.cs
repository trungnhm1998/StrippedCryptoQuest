using System.Collections;
using CryptoQuest.Battle.Events;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects;
using TinyMessenger;
using UnityEngine;

namespace CryptoQuest.Battle
{
    public class HandleLost : MonoBehaviour
    {
        [SerializeField] private BattleUnloader _unloader;
        [SerializeField] private BattleBus _battleBus;
        private TinyMessageSubscriptionToken _turnLostEvent;
        private TinyMessageSubscriptionToken _finishedPresentingEvent;
        private bool _isLost;

        private void Awake()
        {
            _finishedPresentingEvent = BattleEventBus.SubscribeEvent<FinishedPresentingEvent>(_ =>
            {
                if (_isLost) StartCoroutine(CoOnPresentLost());
            });
            _turnLostEvent = BattleEventBus.SubscribeEvent<TurnLostEvent>(_ => _isLost = true);
        }

        private void OnDestroy()
        {
            BattleEventBus.UnsubscribeEvent(_finishedPresentingEvent);
            BattleEventBus.UnsubscribeEvent(_turnLostEvent);
        }

        private IEnumerator CoOnPresentLost()
        {
            yield return _unloader.FadeInAndUnloadBattle();
            BattleEventBus.RaiseEvent(new BattleLostEvent { Battlefield = _battleBus.CurrentBattlefield });
        }
    }
}