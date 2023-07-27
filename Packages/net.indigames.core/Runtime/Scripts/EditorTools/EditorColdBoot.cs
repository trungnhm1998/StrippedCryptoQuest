using IndiGames.Core.Common;
using IndiGames.Core.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace IndiGames.Core.EditorTools
{
    public class EditorColdBoot : MonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField] private SceneScriptableObject _thisSceneSO;
        [SerializeField] private SceneScriptableObject _globalManagersSO;

        [SerializeField]
        private ScriptableObjectAssetReference<LoadSceneEventChannelSO> _editorColdBootEventChannelSO;

        [Header("Raise on")]
        [SerializeField] private VoidEventChannelSO _sceneLoadedEventChannelSO;

        private bool _isStartFromEditor;

        private void Awake()
        {
             _isStartFromEditor =
                 !SceneManager.GetSceneByName(_globalManagersSO.SceneReference.editorAsset.name).isLoaded 
                 && !_globalManagersSO.SceneReference.OperationHandle.IsValid();
        }

        private void Start()
        {
            if (_isStartFromEditor)
                _globalManagersSO.SceneReference.LoadSceneAsync(LoadSceneMode.Additive).Completed +=
                    GlobalManagersSceneLoaded;
        }

        private void GlobalManagersSceneLoaded(AsyncOperationHandle<SceneInstance> obj)
        {
            _editorColdBootEventChannelSO.LoadAssetAsync<LoadSceneEventChannelSO>().Completed +=
                NotifyColdStartupEventChannelLoaded;
        }

        private void NotifyColdStartupEventChannelLoaded(AsyncOperationHandle<LoadSceneEventChannelSO> editorColdBootEventChannelOpHandle)
        {
            if (_thisSceneSO != null)
            {
                editorColdBootEventChannelOpHandle.Result.RequestLoad(_thisSceneSO);
            }
            else
            {
                _sceneLoadedEventChannelSO.RaiseEvent();
            }
        }
#endif
    }
}