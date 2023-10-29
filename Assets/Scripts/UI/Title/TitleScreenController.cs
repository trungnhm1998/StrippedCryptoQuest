using CryptoQuest.Core;
using CryptoQuest.System;
using CryptoQuest.System.SaveSystem;
using IndiGames.Core.SceneManagementSystem.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using TinyMessenger;
using UnityEngine;

namespace CryptoQuest.UI.Title
{
    public class StartGameAction : ActionBase { }

    public class TitleScreenController : MonoBehaviour
    {
        [SerializeField] private SaveSystemSO _save;
        [SerializeField] private SceneScriptableObject _sceneToLoad;

        [Header("Raise on")]
        [SerializeField] private LoadSceneEventChannelSO _loadMapChannel;
        private TinyMessageSubscriptionToken _startGameEvent;

        private void OnEnable()
        {
            _startGameEvent = ActionDispatcher.Bind<StartGameAction>(StartGame);
        }

        private void OnDisable()
        {
            ActionDispatcher.Unbind(_startGameEvent);
        }

        // TODO: get scene scriptable object from _saveSystemSO and load it
        private void HandleContinueGame()
        {
            Debug.LogWarning("Try to load saved game.");
        }

        private void HandleStartNewGame()
        {
            var saveSystem = ServiceProvider.GetService<ISaveSystem>();
            saveSystem?.LoadScene(ref _sceneToLoad);
            _loadMapChannel.RequestLoad(_sceneToLoad);
        }

        private void StartGame(StartGameAction _) { }
    }
}