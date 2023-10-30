using CryptoQuest.Core;
using CryptoQuest.Sagas;
using CryptoQuest.System;
using CryptoQuest.System.SaveSystem;
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

        protected override void HandleAction(StartGameAction ctx)
        {
            
            var saveSystem = ServiceProvider.GetService<ISaveSystem>();
            saveSystem?.LoadScene(ref _defaultStartScene);
            _loadMapChannel.RequestLoad(_defaultStartScene);
        }
    }
}