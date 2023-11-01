using CryptoQuest.Core;
using CryptoQuest.Sagas;
using CryptoQuest.System;
using CryptoQuest.System.SaveSystem;
using CryptoQuest.System.SaveSystem.Actions;
using IndiGames.Core.SceneManagementSystem.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;

namespace CryptoQuest.UI.Title
{
    public class StartGameAction : ActionBase { }

    public class TitleScreenController : SagaBase<StartGameAction>
    {
        [SerializeField] private SaveSystemSO _save;

        [FormerlySerializedAs("_sceneToLoad")] [SerializeField]
        private SceneScriptableObject _defaultStartScene;

        [Header("Raise on")]
        [SerializeField] private LoadSceneEventChannelSO _loadMapChannel;

        private TinyMessenger.TinyMessageSubscriptionToken _listenToLoadCompletedEventToken;

        protected override void HandleAction(StartGameAction ctx)
        {
            var saveSystem = ServiceProvider.GetService<ISaveSystem>();
            _listenToLoadCompletedEventToken = ActionDispatcher.Bind<LoadSceneCompletedAction>(_ => LoadScene());
            ActionDispatcher.Dispatch(new LoadSceneAction(_defaultStartScene));
        }

        protected void LoadScene()
        {
            ActionDispatcher.Unbind(_listenToLoadCompletedEventToken);
            _loadMapChannel.RequestLoad(_defaultStartScene);
        }
    }
}