using Core.SceneManagementSystem.Events.ScriptableObjects;
using Core.SceneManagementSystem.ScriptableObjects;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Core.SceneManagementSystem
{
    public class SceneLoader : MonoBehaviour
    {
        [Header("Listening on")]
        [SerializeField] private LoadSceneEventChannelSO _loadSceneEventChannelSO;

        private SceneScriptableObject _sceneToLoadSO;

        private void OnEnable()
        {
            _loadSceneEventChannelSO.LoadingRequested += MainMenu_LoadingRequested;
        }

        private void OnDisable()
        {
            _loadSceneEventChannelSO.LoadingRequested -= MainMenu_LoadingRequested;
        }

        private void MainMenu_LoadingRequested(SceneScriptableObject mainMenuSceneSO)
        {
            _sceneToLoadSO = mainMenuSceneSO;
            var op = _sceneToLoadSO.SceneReference.LoadSceneAsync(LoadSceneMode.Additive, true, 0);
            op.Completed += OnMainMenuLoaded;
        }

        private void OnMainMenuLoaded(AsyncOperationHandle<SceneInstance> obj)
        {
        }
    }
}