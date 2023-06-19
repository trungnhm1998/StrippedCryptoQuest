using Core.Runtime.Common;
using Core.Runtime.Events.ScriptableObjects;
using Core.Runtime.SceneManagementSystem.Events.ScriptableObjects;
using Core.Runtime.SceneManagementSystem.ScriptableObjects;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Core.Runtime.EditorTools
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
                !SceneManager.GetSceneByName(_globalManagersSO.SceneReference.editorAsset.name).isLoaded;
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

        private void NotifyColdStartupEventChannelLoaded(AsyncOperationHandle<LoadSceneEventChannelSO> obj)
        {
            if (_thisSceneSO != null)
            {
                obj.Result.OnRaiseEvent(_thisSceneSO);
            }
            else
            {
                _sceneLoadedEventChannelSO.OnRaiseEvent();
            }
        }
#endif
    }
}