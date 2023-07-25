using IndiGames.Core.SceneManagementSystem;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;

namespace CryptoQuest.System.SceneManagement
{
    public class SceneLoaderHandler : LinearGameSceneLoader
    {
        public SceneLoaderBus SceneLoadBus;
        public SceneScriptableObject CurrentLoadedScene => _currentLoadedScene;

        private void Awake()
        {
            SceneLoadBus.SceneLoader = this;
        }
    }
}