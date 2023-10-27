using System.Collections;
using IndiGames.Core.Common;
using IndiGames.Core.SceneManagementSystem.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IndiGames.Core.SceneManagementSystem
{
    public class StartupLoader : MonoBehaviour
    {
        [SerializeField] private SceneScriptableObject _managerScene;
        [SerializeField] private SceneScriptableObject _titleScene;
        [SerializeField] private SceneAssetReference _networkManager;

        [Header("Raise on")]
        [SerializeField] private ScriptableObjectAssetReference<LoadSceneEventChannelSO> _loadMainMenuEventChannelSO;

        private IEnumerator Start()
        {
            yield return _managerScene.SceneReference.LoadSceneAsync(LoadSceneMode.Additive);
            var loadTitleEventHandle = _loadMainMenuEventChannelSO.LoadAssetAsync();
            yield return loadTitleEventHandle;
            _networkManager.LoadSceneAsync(LoadSceneMode.Additive);
            loadTitleEventHandle.Result.RequestLoad(_titleScene);
            SceneManager.UnloadSceneAsync(0);
        }
    }
}