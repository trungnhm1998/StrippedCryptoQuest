using CryptoQuest.Battle.Events;
using CryptoQuest.Gameplay.Reward.Events;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CryptoQuest.Battle.PostBattleHandlers
{
    public class WonBattleHandler : PostBattleManager
    {
        [SerializeField] private RewardLootEvent _rewardLootEvent;
        [SerializeField] private BattleResultEventSO _battleWonEvent;

        protected override ResultSO.EState ResultState => ResultSO.EState.Win;

        protected override void HandleResult()
        {
            SceneManager.SetActiveScene(_battleBus.LastActiveScene);
            _battleWonEvent.RaiseEvent(_battleBus.CurrentBattlefield);
            _rewardLootEvent.RaiseEvent(Result.Loots);
        }
    }
}