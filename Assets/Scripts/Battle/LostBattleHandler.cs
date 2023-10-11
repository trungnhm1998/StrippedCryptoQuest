using CryptoQuest.Battle.Events;
using CryptoQuest.Gameplay.Encounter;
using IndiGames.Core.SceneManagementSystem;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using TinyMessenger;
using UnityEngine;

namespace CryptoQuest.Battle
{
    public class LostBattleHandler : PostBattleManager
    {
        [SerializeField] private BattleResultEventSO _battleCompletedEvent;
        private TinyMessageSubscriptionToken _lostToken;

        private void Awake()
        {
            _lostToken = BattleEventBus.SubscribeEvent<BattleLostEvent>(HandleLost);
        }

        private void OnDestroy()
        {
            BattleEventBus.UnsubscribeEvent(_lostToken);
        }

        private BattleLostEvent _context;

        private void HandleLost(BattleLostEvent lostContext)
        {
            _context = lostContext;
            AdditiveGameSceneLoader.SceneUnloaded += TeleportToClosestTownAfterSceneUnloaded;
            UnloadBattleScene();
            NotifyBattleResult(_context.Battlefield);
        }

        private void TeleportToClosestTownAfterSceneUnloaded(SceneScriptableObject scene)
        {
            AdditiveGameSceneLoader.SceneUnloaded -= TeleportToClosestTownAfterSceneUnloaded;
            if (scene != BattleSceneSO) return;

            FinishPresentationAndEnableInput();
        }

        private void NotifyBattleResult(Battlefield battlefield)
        {
            BattleResultInfo result = new()
            {
                IsWin = false, //obsolete 
                Battlefield = battlefield
            };
            _battleCompletedEvent.RaiseEvent(result);
        }
    }
}