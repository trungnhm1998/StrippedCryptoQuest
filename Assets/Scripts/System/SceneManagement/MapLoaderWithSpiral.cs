using IndiGames.Core.SceneManagementSystem.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.System.SceneManagement
{
    public class MapLoaderWithSpiral : MonoBehaviour
    {
        [SerializeField] private SceneLoaderHandler _sceneLoaderHandler;
        [SerializeField] private LoadSceneEventChannelSO _loadMapWithSpiralEventChannel;

        private void OnEnable()
        {
            _loadMapWithSpiralEventChannel.LoadingRequested += RequestLoadMap;
        }

        private void OnDisable()
        {
            _loadMapWithSpiralEventChannel.LoadingRequested -= RequestLoadMap;
        }

        private void RequestLoadMap(SceneScriptableObject sceneScriptableObject)
        {
            _sceneLoaderHandler.MapLoadingRequestedWithSpiralFX(sceneScriptableObject);
        }
    }
}