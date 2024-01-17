using System.Collections;
using CryptoQuest.Battle.Events;
using TinyMessenger;
using UnityEngine;

namespace CryptoQuest.Battle.PostBattleHandlers
{
    /// <summary>
    /// This should live on gameplay manager and wait until battle scene finished clean up
    /// Reward if won or teleport to closest town if lost after battle scene unloaded then will fade out
    /// </summary>

    [RequireComponent(typeof(BattleUnloadHandler))]
    public abstract class PostBattleManager : MonoBehaviour
    {
        [SerializeField] private BattleUnloadHandler _unloadHandler;
        [SerializeField] private ResultSO _resultSO;
        [SerializeField] protected BattleBus _battleBus;
        protected ResultSO Result => _resultSO;

        private TinyMessageSubscriptionToken _battleEnded;

        protected abstract ResultSO.EState ResultState { get; }

        private void OnValidate()
        {
            _unloadHandler = GetComponent<BattleUnloadHandler>();
        }

        private void OnEnable()
        {
            _battleEnded = BattleEventBus.SubscribeEvent<BattleCleanUpFinishedEvent>((_) =>
                StartCoroutine(CoFinishPresentation()));
        }

        private void OnDisable()
        {
            BattleEventBus.UnsubscribeEvent(_battleEnded);
        }

        private IEnumerator CoFinishPresentation()
        {
            if (_resultSO.State != ResultState) yield break;
            yield return _unloadHandler.UnloadBattle();
            HandleResult();
        }

        protected virtual void HandleResult() { }
    }
}