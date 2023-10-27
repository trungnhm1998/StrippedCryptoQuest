using CryptoQuest.System;
using IndiGames.Core.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.UI.Title
{
    public class TitleScreenController : MonoBehaviour
    {
        [SerializeField] private SceneScriptableObject _sceneToLoad;

        [Header("Listen on")]
        [SerializeField] private VoidEventChannelSO _startNewGameChannel;
        [SerializeField] private VoidEventChannelSO _continueGameChannel;

        [Header("Raise on")]
        [SerializeField] private LoadSceneEventChannelSO _loadMapChannel;

        private void OnEnable()
        {
            _startNewGameChannel.EventRaised += HandleStartNewGame;
            _continueGameChannel.EventRaised += HandleContinueGame;
        }

        private void OnDisable()
        {
            _startNewGameChannel.EventRaised -= HandleStartNewGame;
            _continueGameChannel.EventRaised -= HandleContinueGame;
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
    }
}