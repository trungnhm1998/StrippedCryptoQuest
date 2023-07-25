using IndiGames.Core.SceneManagementSystem;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;

namespace CryptoQuest.System.SceneManagement
{
    public class SceneLoaderHandler : LinearGameSceneLoader
    {
        [SerializeField] private SceneLoaderBus _sceneLoadBus;
        public SceneScriptableObject CurrentLoadedScene => _currentLoadedScene;

        private void Awake()
        {
            _sceneLoadBus.SceneLoader = this;
        }
    }
}