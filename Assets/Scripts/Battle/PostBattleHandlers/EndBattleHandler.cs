using CryptoQuest.Battle.Events;
using CryptoQuest.Gameplay.Reward.Events;
using IndiGames.Core.SceneManagementSystem;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using TinyMessenger;
using UnityEngine;

namespace CryptoQuest.Battle
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
            BattleEventBus.UnsubscribeEvent(_endEventToken);
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