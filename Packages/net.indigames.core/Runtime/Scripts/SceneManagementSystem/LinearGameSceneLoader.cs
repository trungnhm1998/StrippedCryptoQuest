using System.Collections;
using IndiGames.Core.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using IndiGames.Core.UI;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace IndiGames.Core.SceneManagementSystem
{
    // TODO: Class current violate SRP move fading logic to separate class
    public class LinearGameSceneLoader : MonoBehaviour
    {
        [SerializeField] private SceneScriptableObject _gameplayManagerSceneSO;
        [SerializeField] private FadeConfigSO _fadeConfigSO;

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
        protected SceneScriptableObject SceneToLoad => _sceneToLoad;

        private SceneScriptableObject _currentlyLoadedScene; // ignore gameplay/global manager scene
        private SceneInstance _gameplayManagerSceneInstance;
        private bool _isLoading;

        protected virtual void OnEnable()
        {
            _loadMap.LoadingRequested += MapLoadingRequested;
            _loadTitle.LoadingRequested += LoadTitleScene;
#if UNITY_EDITOR
            _editorColdBoot.LoadingRequested += EditorColdBootLoadingRequested;
#endif
        }

        protected virtual void OnDisable()
        {
            _loadMap.LoadingRequested -= MapLoadingRequested;
            _loadTitle.LoadingRequested -= LoadTitleScene;
#if UNITY_EDITOR
            _editorColdBoot.LoadingRequested -= EditorColdBootLoadingRequested;
#endif
        }

        private void MapLoadingRequested(SceneScriptableObject gameplaySceneToLoad)
        {
            if (_isLoading) return;
            _isLoading = true;
            StartCoroutine(CoLoadGameplayScene(gameplaySceneToLoad));
        }

        /// <summary>
        /// Gameplay scene will need GameplayManager scene to be loaded first
        /// </summary>
        /// <returns></returns>
        private IEnumerator CoLoadGameplayScene(SceneScriptableObject gameplayScene)
        {
            yield return CoLoadGameplayManagerSceneIfNotLoaded();
            yield return CoUnloadPreviousScene();
            yield return CoLoadNextScene(gameplayScene);
        }

        private void LoadTitleScene(SceneScriptableObject mainMenu)
            => StartCoroutine(CoLoadNextScene(mainMenu));

#if UNITY_EDITOR
        /// <summary>
        /// Support start up from any scene in editor
        /// </summary>
        private void EditorColdBootLoadingRequested(SceneScriptableObject currentOpenSceneInEditor)
            => StartCoroutine(Editor_CoSetupScene(currentOpenSceneInEditor));

        private IEnumerator Editor_CoSetupScene(SceneScriptableObject sceneOpenedDirectlyFromEditor)
        {
            var sceneOpenedFromEditor = SceneManager.GetActiveScene();
            _sceneToLoad = sceneOpenedDirectlyFromEditor;
            if (_sceneToLoad.SceneType == SceneScriptableObject.Type.Location)
                yield return CoLoadGameplayManagerSceneIfNotLoaded();
            
            // The currently scene already loaded when open directly through EditorColdBoot
            // Skip loading and just raised the event
            OnSceneLoaded(sceneOpenedFromEditor);
        }
#endif

        /// <summary>
        /// Loading gameplay manager if needed, for gameplay scene which need gameplay manager
        /// </summary>
        private IEnumerator CoLoadGameplayManagerSceneIfNotLoaded()
        {
            if (_gameplayManagerSceneInstance.Scene.isLoaded) yield break;
            _gameplayManagerLoadingOperationHandle =
                _gameplayManagerSceneSO.SceneReference.LoadSceneAsync(LoadSceneMode.Additive);
            yield return _gameplayManagerLoadingOperationHandle;
            if (_gameplayManagerLoadingOperationHandle.Result.Scene.IsValid() == false)
            {
                Debug.LogError("Failed to load Gameplay Manager Scene");
                yield break;
            }

            _gameplayManagerSceneInstance = _gameplayManagerLoadingOperationHandle.Result;
        }

        private IEnumerator CoLoadNextScene(SceneScriptableObject sceneToLoad)
        {
            _sceneToLoad = sceneToLoad;
            _sceneLoadingOperationHandle =
                _sceneToLoad.SceneReference.LoadSceneAsync(LoadSceneMode.Additive, true, 0);
            yield return _sceneLoadingOperationHandle;

            OnSceneLoaded(_sceneLoadingOperationHandle.Result.Scene);
        }

        private IEnumerator CoUnloadPreviousScene()
        {
            _sceneUnloading.RaiseEvent();
            _fadeConfigSO.OnFadeIn();
            yield return new WaitForSeconds(_fadeConfigSO.Duration);
            if (_currentlyLoadedScene != null)
            {
                if (_currentlyLoadedScene.SceneReference.OperationHandle.IsValid())
                    _currentlyLoadedScene.SceneReference.UnLoadScene();
#if UNITY_EDITOR
                else UnloadSceneWhenStartFromEditor();
#endif
            }

            Resources.UnloadUnusedAssets();
        }

#if UNITY_EDITOR
        /// <summary>
        /// <see cref="_sceneLoadingOperationHandle"/> will be null because the scene is
        /// already loaded when open directly through EditorColdBoot, we not using using any logic to load the scene
        /// </summary>
        private void UnloadSceneWhenStartFromEditor()
            => SceneManager.UnloadSceneAsync(_currentlyLoadedScene.SceneReference.editorAsset.name);
#endif

        protected virtual void OnSceneLoaded(Scene scene)
        {
            _currentlyLoadedScene = _sceneToLoad;
            SceneManager.SetActiveScene(scene);
            _isLoading = false;
            _fadeConfigSO.OnFadeOut(); // TODO: Some scene need to init their logic before fade out
            _sceneLoaded.RaiseEvent();
        }
    }
}