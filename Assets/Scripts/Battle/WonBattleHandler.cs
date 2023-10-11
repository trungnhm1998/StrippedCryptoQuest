using CryptoQuest.Battle.Events;
using CryptoQuest.Gameplay.Encounter;
using CryptoQuest.Gameplay.Reward.Events;
using IndiGames.Core.SceneManagementSystem;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using TinyMessenger;
using UnityEngine;

namespace CryptoQuest.Battle
{
    public class WonBattleHandler : PostBattleManager
    {
        [SerializeField] private RewardLootEvent _rewardLootEvent;
        [SerializeField] private BattleResultEventSO _battleCompletedEvent;
        private TinyMessageSubscriptionToken _wonToken;

        private void Awake()
        {
            _wonToken = BattleEventBus.SubscribeEvent<BattleWonEvent>(HandleWon);
        }

        private void OnDestroy()
        {
            BattleEventBus.UnsubscribeEvent(_wonToken);
        }

        private BattleWonEvent _context;

        private void HandleWon(BattleWonEvent wonContext)
        {
            _context = wonContext;
            AdditiveGameSceneLoader.SceneUnloaded += RewardAfterSceneUnloaded;
            UnloadBattleScene();
            NotifyBattleResult(_context.Battlefield);
        }


        private void RewardAfterSceneUnloaded(SceneScriptableObject scene)
        {
            AdditiveGameSceneLoader.SceneUnloaded -= RewardAfterSceneUnloaded;
            if (scene != BattleSceneSO) return;
            FinishPresentationAndEnableInput();
        }

        protected override void OnFadeOut()
        {
            _rewardLootEvent.RaiseEvent(_context.Loots);
        }

        private void NotifyBattleResult(Battlefield battlefield)
        {
            BattleResultInfo result = new()
            {
                IsWin = true, //obsolete 
                Battlefield = battlefield
            };
            _battleCompletedEvent.RaiseEvent(result);
        }
    }
}