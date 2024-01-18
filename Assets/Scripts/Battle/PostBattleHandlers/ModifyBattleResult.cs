using CryptoQuest.Battle.Events;
using TinyMessenger;
using UnityEngine;

namespace CryptoQuest.Battle.PostBattleHandlers
{
    /// <summary>
    /// As long as this mono is in the scene it will modify the battle result to any result
    /// </summary>
    public class ModifyBattleResult : MonoBehaviour
    {
        [SerializeField] private ResultSO.EState _resultToModify;
        [SerializeField] private ResultSO _resultSO;
        private TinyMessageSubscriptionToken _finishedPresentingToken;

        private void OnEnable()
        {
            _finishedPresentingToken = BattleEventBus.SubscribeEvent<FinishedPresentingEvent>(ModifyResult);
        }

        private void OnDisable()
        {
            BattleEventBus.UnsubscribeEvent(_finishedPresentingToken);
        }

        /// <summary>
        /// Only modify when battle end
        /// </summary>
        /// <param name="_"></param>
        private void ModifyResult(FinishedPresentingEvent _)
        {
            if (_resultSO.State == ResultSO.EState.None) return;
            _resultSO.State = _resultToModify;
        }
    }
}