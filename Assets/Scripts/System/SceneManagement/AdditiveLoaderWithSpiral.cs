using IndiGames.Core.SceneManagementSystem.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.System.SceneManagement
{
    public class AdditiveLoaderWithSpiral : MonoBehaviour
    {
        [SerializeField] private SceneLoaderHandler _sceneLoaderHandler;
        [SerializeField] private LoadSceneEventChannelSO _loadAdditiveWithSpiralEventChannel;

        private void OnEnable()
        {
            _loadAdditiveWithSpiralEventChannel.LoadingRequested += RequestLoadAdditive;
        }

        private void OnDisable()
        {
            _loadAdditiveWithSpiralEventChannel.LoadingRequested -= RequestLoadAdditive;
        }

        private void RequestLoadAdditive(SceneScriptableObject sceneScriptableObject)
        {
            _sceneLoaderHandler.AdditiveSceneLoadingWithSpiralRequested(sceneScriptableObject);
        }
    }
}