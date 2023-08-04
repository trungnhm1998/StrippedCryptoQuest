using System.Collections;
using System.Collections.Generic;
using IndiGames.Core.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using IndiGames.Core.UI.FadeController;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace IndiGames.Core.SceneManagementSystem
{
    public class AdditiveGameSceneLoader : MonoBehaviour
    {
        [SerializeField] private SceneScriptableObject _gameplayManagerSceneSO;
        [SerializeField] private FadeConfigSO _fadeConfigSO;

        [Header("Listening on")]
        [SerializeField] private LoadSceneEventChannelSO _loadSceneEvent;
        [SerializeField] private UnloadSceneEventChannelSO _unloadSceneEvent;

#if UNITY_EDITOR
        [SerializeField] private LoadSceneEventChannelSO _editorColdBoot;
#endif

        [Header("Raise on")]
        [SerializeField] private VoidEventChannelSO _sceneLoaded;

        [SerializeField] private VoidEventChannelSO _sceneUnloading;
        
        private AsyncOperationHandle<SceneInstance> _sceneLoadingOperationHandle;
        private AsyncOperationHandle<SceneInstance> _gameplayManagerLoadingOperationHandle;

        private SceneScriptableObject _sceneToLoad;

        private SceneInstance _gameplayManagerSceneInstance;
        private bool _isLoading;

        private void OnEnable()
        {
            _loadSceneEvent.LoadingRequested += SceneLoadingRequested;
            _unloadSceneEvent.UnloadRequested += UnloadSceneRequested;
#if UNITY_EDITOR
            _editorColdBoot.LoadingRequested += EditorColdBootLoadingRequested;
#endif
        }

        private void OnDisable()
        {
            _loadSceneEvent.LoadingRequested -= SceneLoadingRequested;
            _unloadSceneEvent.UnloadRequested -= UnloadSceneRequested;
#if UNITY_EDITOR
            _editorColdBoot.LoadingRequested -= EditorColdBootLoadingRequested;
#endif
        }

        private void SceneLoadingRequested(SceneScriptableObject sceneToLoad)
        {
            if (_isLoading) return;
            
            _fadeConfigSO.OnFadeIn();

            _sceneToLoad = sceneToLoad;
            _isLoading = true;

            if (!_gameplayManagerSceneInstance.Scene.isLoaded && !_gameplayManagerSceneSO.SceneReference.OperationHandle.IsValid())
            {
                LoadGameplayManagerScene();
                return;
            }

            LoadNewScene();
        }

#if UNITY_EDITOR
        private void EditorColdBootLoadingRequested(SceneScriptableObject currentOpenSceneInEditor)
        {
           StartCoroutine(CoLoadScene(currentOpenSceneInEditor)); 
        }
        
        private IEnumerator CoLoadScene(SceneScriptableObject currentOpenSceneInEditor)
        {
            if (currentOpenSceneInEditor.SceneType == SceneScriptableObject.Type.Location)
            {
                _gameplayManagerLoadingOperationHandle =
                    _gameplayManagerSceneSO.SceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);
                yield return _gameplayManagerLoadingOperationHandle; 
                if (!_gameplayManagerLoadingOperationHandle.Result.Scene.IsValid()) yield break;
                _gameplayManagerSceneInstance = _gameplayManagerLoadingOperationHandle.Result;
            }

            _sceneLoaded.RaiseEvent();
        }
#endif

        /// <summary>
        /// Using callback because WebGL doesn't support threading for async/await
        /// </summary>
        private void LoadGameplayManagerScene()
        {
            _gameplayManagerLoadingOperationHandle =
                _gameplayManagerSceneSO.SceneReference.LoadSceneAsync(LoadSceneMode.Additive);
            _gameplayManagerLoadingOperationHandle.Completed += GameplayManagerSceneLoaded;
        }

        private void GameplayManagerSceneLoaded(AsyncOperationHandle<SceneInstance> asyncOpSceneInstance)
        {
            _gameplayManagerSceneInstance = asyncOpSceneInstance.Result;
            LoadNewScene();
        }

        private void UnloadSceneRequested(SceneScriptableObject sceneToUnload)
        {
            _sceneUnloading.RaiseEvent();
            StartCoroutine(CoUnloadPreviousScene(sceneToUnload));
        }

        private IEnumerator CoUnloadPreviousScene(SceneScriptableObject sceneToUnload)
        {
            _fadeConfigSO.OnFadeIn();
            yield return new WaitForSeconds(_fadeConfigSO.Duration);
            if (sceneToUnload != null)
            {
                if (sceneToUnload.SceneReference.OperationHandle.IsValid())
                {
                    sceneToUnload.SceneReference.UnLoadScene();
                }
#if UNITY_EDITOR
                else
                {
                    SceneManager.UnloadSceneAsync(sceneToUnload.SceneReference.editorAsset.name);
                }
#endif
            }
            _fadeConfigSO.OnFadeOut();
        }

        private void LoadNewScene()
        {
            _sceneLoadingOperationHandle =
                _sceneToLoad.SceneReference.LoadSceneAsync(LoadSceneMode.Additive, true, 0);
            _sceneLoadingOperationHandle.Completed += NewSceneLoaded;
        }

        private void NewSceneLoaded(AsyncOperationHandle<SceneInstance> asyncOpSceneInstance)
        {
            var scene = asyncOpSceneInstance.Result.Scene;
            SceneManager.SetActiveScene(scene);

            _isLoading = false;

            _fadeConfigSO.OnFadeOut();

            _sceneLoaded.RaiseEvent();
        }
    }
}