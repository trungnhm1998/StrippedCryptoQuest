using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CryptoQuest.Battle.Events;
using CryptoQuest.Gameplay.Loot;
using CryptoQuest.Gameplay.Reward;
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
            yield return _unloader.FadeInAndUnloadBattle();
            var loots = new List<LootInfo>();
            foreach (var enemy in _context.Enemies.Where(enemy => enemy.Spec.IsValid()))
            {
                loots.AddRange(enemy.GetDroppedLoots());
            }

            loots = RewardManager.CloneAndMergeLoots(loots);
            BattleEventBus.RaiseEvent(new BattleWonEvent()
            {
                Battlefield = _context.CurrentBattlefield,
                Loots = loots
            });
        }
    }
}