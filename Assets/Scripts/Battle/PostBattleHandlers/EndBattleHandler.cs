using CryptoQuest.Battle.Events;
using IndiGames.Core.SceneManagementSystem;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using TinyMessenger;

namespace CryptoQuest.Battle.PostBattleHandlers
{
    public class EndBattleHandler : PostBattleManager
    {
        private TinyMessageSubscriptionToken _endEventToken;

        private void Awake()
        {
            _endEventToken = BattleEventBus.SubscribeEvent<BattleRetreatedEvent>(HandleEnd);
        }

        private void OnDestroy()
        {
            if (_endEventToken != null) BattleEventBus.UnsubscribeEvent(_endEventToken);
        }

        private void HandleEnd(BattleRetreatedEvent context)
        {
            AdditiveGameSceneLoader.SceneUnloaded += UnloadedScene;
            UnloadBattleScene();
        }

        private void UnloadedScene(SceneScriptableObject scene)
        {
            AdditiveGameSceneLoader.SceneUnloaded -= UnloadedScene;
            if (scene != BattleSceneSO) return;
            FinishPresentationAndEnableInput();
        }
    }
}