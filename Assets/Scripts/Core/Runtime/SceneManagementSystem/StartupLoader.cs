using Core.Runtime.Common;
using Core.Runtime.SceneManagementSystem.Events.ScriptableObjects;
using Core.Runtime.SceneManagementSystem.ScriptableObjects;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Core.Runtime.SceneManagementSystem
{
    public class StartupLoader : MonoBehaviour
    {
        [SerializeField] private SceneScriptableObject _managerScene;
        [SerializeField] private SceneScriptableObject _titleScene;

        [Header("Raise on")]
        [SerializeField] private ScriptableObjectAssetReference<LoadSceneEventChannelSO> _loadMainMenuEventChannelSO;

        private void Start()
        {
            _managerScene.SceneReference.LoadSceneAsync(LoadSceneMode.Additive).Completed += OnManagerSceneLoaded;
        }

        private void OnManagerSceneLoaded(AsyncOperationHandle<SceneInstance> sceneInstanceAsyncOperationHandle)
        {
            _loadMainMenuEventChannelSO.LoadAssetAsync().Completed += OnLoadMainMenuEventChannelSOLoaded;
        }

        private void OnLoadMainMenuEventChannelSOLoaded(
            AsyncOperationHandle<LoadSceneEventChannelSO> loadMainMenuEventChannel)
        {
            loadMainMenuEventChannel.Result.RequestLoad(_titleScene);

            SceneManager.UnloadSceneAsync(0);
        }
    }
}