using System.Collections;
using Core.Runtime.Events.ScriptableObjects;
using Core.Runtime.SceneManagementSystem.Events.ScriptableObjects;
using Core.Runtime.SceneManagementSystem.ScriptableObjects;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Core.Runtime.SceneManagementSystem
{
    public class LinearGameSceneLoader : MonoBehaviour
    {
        [SerializeField] private SceneScriptableObject _gameplayManagerSceneSO;
        [SerializeField] private float _fadeDuration = .3f;

        [Header("Listening on")]
        [SerializeField] private LoadSceneEventChannelSO _loadMap;

        [SerializeField] private LoadSceneEventChannelSO _loadTitle;
#if UNITY_EDITOR
        [SerializeField] private LoadSceneEventChannelSO _editorColdBoot;
#endif

        [Header("Raise on")]
        [SerializeField] private VoidEventChannelSO _sceneLoaded;

        [SerializeField] private VoidEventChannelSO _sceneUnloading;

        private AsyncOperationHandle<SceneInstance> _sceneLoadingOperationHandle;
        private AsyncOperationHandle<SceneInstance> _gameplayManagerLoadingOperationHandle;

        private SceneScriptableObject _sceneToLoad;
        private SceneScriptableObject _currentLoadedScene;

        private SceneInstance _gameplayManagerSceneInstance;
        private bool _isLoading;

        private void OnEnable()
        {
            _loadMap.LoadingRequested += MapLoadingRequested;
            _loadTitle.LoadingRequested += TitleSceneLoadingRequested;
#if UNITY_EDITOR
            _editorColdBoot.LoadingRequested += EditorColdBootLoadingRequested;
#endif
        }

        private void OnDisable()
        {
            _loadMap.LoadingRequested -= MapLoadingRequested;
            _loadTitle.LoadingRequested -= TitleSceneLoadingRequested;
#if UNITY_EDITOR
            _editorColdBoot.LoadingRequested -= EditorColdBootLoadingRequested;
#endif
        }

        private void MapLoadingRequested(SceneScriptableObject locationToLoad)
        {
            if (_isLoading) return;

            _sceneToLoad = locationToLoad;
            _isLoading = true;

            if (!_gameplayManagerSceneInstance.Scene.isLoaded)
            {
                _gameplayManagerLoadingOperationHandle =
                    _gameplayManagerSceneSO.SceneReference.LoadSceneAsync(LoadSceneMode.Additive);
                _gameplayManagerLoadingOperationHandle.Completed += GameplayManagerSceneLoaded;
            }
            else
            {
                UnloadPreviousScene();
            }
        }

        private void TitleSceneLoadingRequested(SceneScriptableObject mainMenu)
        {
            _sceneToLoad = mainMenu;

            if (_gameplayManagerSceneInstance.Scene.isLoaded)
                UnloadPreviousScene();
            else
                LoadGameplayManagerScene();
        }

#if UNITY_EDITOR
        private void EditorColdBootLoadingRequested(SceneScriptableObject currentOpenSceneInEditor)
        {
            _currentLoadedScene = currentOpenSceneInEditor;

            if (_currentLoadedScene.SceneType != SceneScriptableObject.Type.Location) return;

            _gameplayManagerLoadingOperationHandle =
                _gameplayManagerSceneSO.SceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);
            _gameplayManagerLoadingOperationHandle.WaitForCompletion();
            _gameplayManagerSceneInstance = _gameplayManagerLoadingOperationHandle.Result;

            _sceneLoaded.RaiseEvent();
        }
#endif

        /// <summary>
        /// Using <c>.Completed += </c> because WebGL doesn't support threading for async/await
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

            UnloadPreviousScene();
        }

        private void UnloadPreviousScene()
        {
            _sceneUnloading.RaiseEvent();
            StartCoroutine(CoUnloadPreviousScene());
        }

        private IEnumerator CoUnloadPreviousScene()
        {
            yield return new WaitForSeconds(_fadeDuration);
            if (_currentLoadedScene != null)
            {
                if (_currentLoadedScene.SceneReference.OperationHandle.IsValid())
                {
                    _currentLoadedScene.SceneReference.UnLoadScene();
                }
#if UNITY_EDITOR
                else
                {
                    UnloadSceneWhenStartFromEditor();
                }
#endif
            }

            LoadNewScene();
        }

#if UNITY_EDITOR
        /// <summary>
        /// <c>_currentLoadedSceneSO.SceneReference.OperationHandle</c> would be null because the scene
        /// already loaded when open directly through EditorColdBoot
        /// </summary>
        private void UnloadSceneWhenStartFromEditor()
        {
            SceneManager.UnloadSceneAsync(_currentLoadedScene.SceneReference.editorAsset.name);
        }
#endif

        private void LoadNewScene()
        {
            _sceneLoadingOperationHandle =
                _sceneToLoad.SceneReference.LoadSceneAsync(LoadSceneMode.Additive, true, 0);
            _sceneLoadingOperationHandle.Completed += NewSceneLoaded;
        }


        private void NewSceneLoaded(AsyncOperationHandle<SceneInstance> asyncOpSceneInstance)
        {
            _currentLoadedScene = _sceneToLoad;

            var scene = asyncOpSceneInstance.Result.Scene;
            SceneManager.SetActiveScene(scene);

            _sceneLoaded.RaiseEvent();
        }
    }
}