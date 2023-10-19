using System.Linq;
using CryptoQuest.Battle.Events;
using TinyMessenger;
using UnityEngine;

namespace CryptoQuest.Battle.Presenter
{
    public class EnemiesPresenter : MonoBehaviour
    {
        [SerializeField] private BattleContext _battleContext;
        private TinyMessageSubscriptionToken _highlightEnemyEvent;
        private TinyMessageSubscriptionToken _resetHighlightEnemyEvent;
        private TinyMessageSubscriptionToken _roundStartedEvent;

        private void Awake()
        {
            _roundStartedEvent = BattleEventBus.SubscribeEvent<RoundStartedEvent>(_ => ChangeAllEnemiesOpacity(1f));
            _highlightEnemyEvent = BattleEventBus.SubscribeEvent<HighlightEnemyEvent>(HighlightEnemy);
            _resetHighlightEnemyEvent = BattleEventBus.SubscribeEvent<ResetHighlightEnemyEvent>(ResetHighlightEnemy);
        }

        private void OnDestroy()
        {
            BattleEventBus.UnsubscribeEvent(_roundStartedEvent);
            BattleEventBus.UnsubscribeEvent(_highlightEnemyEvent);
            BattleEventBus.UnsubscribeEvent(_resetHighlightEnemyEvent);
        }

        private void ChangeAllEnemiesOpacity(float f)
        {
            foreach (var enemy in _battleContext.Enemies.Where(enemy => enemy.IsValidAndAlive())) enemy.SetAlpha(f);
        }

        private void HighlightEnemy(HighlightEnemyEvent eventObject)
        {
            ChangeAllEnemiesOpacity(0.5f);
            eventObject.Enemy.SetAlpha(1f);
        }

        private void ResetHighlightEnemy(ResetHighlightEnemyEvent eventObject) => ChangeAllEnemiesOpacity(1f);
    }
}