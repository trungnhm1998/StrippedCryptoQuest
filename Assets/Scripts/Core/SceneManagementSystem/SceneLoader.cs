using CryptoQuest.Core.SceneManagementSystem.Events.ScriptableObjects;
using CryptoQuest.Core.SceneManagementSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Core.SceneManagementSystem
{
    public class SceneLoader : MonoBehaviour
    {
        [Header("Listening on")]
        [SerializeField] private LoadSceneEventChannelSO _loadSceneEventChannelSO;

        private SceneScriptableObject _currentSceneSO;
        private SceneScriptableObject _mainMenuSceneSO;

        private void OnEnable()
        {
            _loadSceneEventChannelSO.LoadingRequested += LoadingMainMenuRequested;
        }

        private void OnDisable()
        {
            _loadSceneEventChannelSO.LoadingRequested -= LoadingMainMenuRequested;
        }

        private void LoadingMainMenuRequested(SceneScriptableObject mainMenuSceneSO)
        {
            _mainMenuSceneSO = mainMenuSceneSO;
        }
    }
}