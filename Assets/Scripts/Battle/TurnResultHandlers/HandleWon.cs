using System.Collections;
using CryptoQuest.Battle.Events;
using TinyMessenger;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.Battle
{
    public class HandleWon : MonoBehaviour
    {
        [SerializeField] private UnityEvent _onWon;
        [SerializeField] private BattleUnloader _unloader;
        [SerializeField] private BattleContext _context;
        [SerializeField] private BattleLootManager _battleLootManager;

        private TinyMessageSubscriptionToken _wonEvent;
        private TinyMessageSubscriptionToken _finishedPresentingEvent;

        private bool _hasWon;

        private void Awake()
        {
            _finishedPresentingEvent = BattleEventBus.SubscribeEvent<FinishedPresentingEvent>(PresentLoseBattle);
            _wonEvent = BattleEventBus.SubscribeEvent<TurnWonEvent>(CacheState);
        }

        private void OnDestroy()
        {
            BattleEventBus.UnsubscribeEvent(_finishedPresentingEvent);
            BattleEventBus.UnsubscribeEvent(_wonEvent);
        }

        private void CacheState(TurnWonEvent _)
        {
            _hasWon = true;
            _onWon.Invoke();
        }

        private void PresentLoseBattle(FinishedPresentingEvent finishedPresentingEvent)
        {
            if (_hasWon == false) return;
            StartCoroutine(CoOnPresentWon());
        }

        private IEnumerator CoOnPresentWon()
        {
            var loots = _battleLootManager.GetDroppedLoots();
            yield return _unloader.FadeInAndUnloadBattle();
            BattleEventBus.RaiseEvent(new BattleWonEvent()
            {
                Battlefield = _context.CurrentBattlefield,
                Loots = loots
            });
        }
    }
}