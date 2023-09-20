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

        [Header("Raise on")]
        [SerializeField] private ScriptableObjectAssetReference<LoadSceneEventChannelSO> _loadMainMenuEventChannelSO;

        private IEnumerator Start()
        {
            yield return _managerScene.SceneReference.LoadSceneAsync(LoadSceneMode.Additive);
            var loadTitleEventHandle = _loadMainMenuEventChannelSO.LoadAssetAsync();
            yield return loadTitleEventHandle;
            loadTitleEventHandle.Result.RequestLoad(_titleScene);
            SceneManager.UnloadSceneAsync(0);
        }
    }
}