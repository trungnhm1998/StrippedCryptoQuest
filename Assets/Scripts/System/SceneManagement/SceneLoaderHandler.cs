using IndiGames.Core.SceneManagementSystem;
using UnityEngine;

namespace CryptoQuest.System.SceneManagement
{
    public class SceneLoaderHandler : LinearGameSceneLoader
    {
        [SerializeField] private SceneLoaderBus _sceneLoadBus;

        private void Awake()
        {
            _sceneLoadBus.SceneLoader = this;
        }
    }
}