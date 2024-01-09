using CryptoQuest.Battle.Events;
using CryptoQuest.Gameplay.Reward.Events;
using IndiGames.Core.SceneManagementSystem;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using TinyMessenger;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CryptoQuest.Battle.PostBattleHandlers
{
    public class WonBattleHandler : PostBattleManager
    {
        [SerializeField] private RewardLootEvent _rewardLootEvent;
        [SerializeField] private BattleResultEventSO _battleWonEvent;
        private TinyMessageSubscriptionToken _wonToken;

        private void Awake()
        {
            _wonToken = BattleEventBus.SubscribeEvent<BattleWonEvent>(HandleWon);
        }

        private void OnDestroy()
        {
            if (_wonToken != null) BattleEventBus.UnsubscribeEvent(_wonToken);
        }

        private BattleWonEvent _context;

        private void HandleWon(BattleWonEvent wonContext)
        {
            _context = wonContext;
            _battleWonEvent.RaiseEvent(wonContext.Battlefield);
            AdditiveGameSceneLoader.SceneUnloaded += RewardAfterSceneUnloaded;
            UnloadBattleScene();
        }

        private void RewardAfterSceneUnloaded(SceneScriptableObject scene)
        {
            AdditiveGameSceneLoader.SceneUnloaded -= RewardAfterSceneUnloaded;
            if (scene != BattleSceneSO) return;
            SceneManager.SetActiveScene(_battleBus.LastActiveScene);
            FinishPresentationAndEnableInput();
        }

        protected override void OnFadeOut()
        {
            _rewardLootEvent.RaiseEvent(_context.Loots);
        }
    }
}