using CryptoQuest.Gameplay.Reward.Events;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CryptoQuest.Battle.PostBattleHandlers
{
    public class WonBattleHandler : PostBattleManager
    {
        [SerializeField] private RewardLootEvent _rewardLootEvent;

        protected override ResultSO.EState ResultState => ResultSO.EState.Win;

        protected override void HandleResult()
        {
            SceneManager.SetActiveScene(_battleBus.LastActiveScene);
            _rewardLootEvent.RaiseEvent(Result.Loots);
        }
    }
}