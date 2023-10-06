using CryptoQuest.Battle.Events;
using IndiGames.Core.SceneManagementSystem;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using TinyMessenger;

namespace CryptoQuest.Battle
{
    public class LostBattleHandler : PostBattleManager
    {
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
        }

        private void TeleportToClosestTownAfterSceneUnloaded(SceneScriptableObject scene)
        {
            AdditiveGameSceneLoader.SceneUnloaded -= TeleportToClosestTownAfterSceneUnloaded;
            if (scene != BattleSceneSO) return;

            FinishPresentationAndEnableInput();
        }
    }
}