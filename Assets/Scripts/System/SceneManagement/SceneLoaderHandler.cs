using System.Collections;
using CryptoQuest.UI.SpiralFX;
using IndiGames.Core.SceneManagementSystem;
using IndiGames.Core.SceneManagementSystem.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace CryptoQuest.System.SceneManagement
{
    public class SceneLoaderHandler : LinearGameSceneLoader
    {
        [SerializeField] private SceneLoaderBus _sceneLoadBus;
        [SerializeField] private LoadSceneEventChannelSO _loadMapWithSpiralEventChannel;
        [SerializeField] private SpiralConfigSO _spiralConfigSO;
        public SceneScriptableObject CurrentLoadedScene => _currentLoadedScene;

        private void Awake()
        {
            _sceneLoadBus.SceneLoader = this;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _loadMapWithSpiralEventChannel.LoadingRequested += MapLoadingRequestedWithSpiralFX;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _loadMapWithSpiralEventChannel.LoadingRequested -= MapLoadingRequestedWithSpiralFX;
        }

        private void MapLoadingRequestedWithSpiralFX(SceneScriptableObject locationToLoad)
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
                UnloadPreviousSceneWithSpiralFX();
            }
        }

        private void UnloadPreviousSceneWithSpiralFX()
        {
            _sceneUnloading.RaiseEvent();
            StartCoroutine(CoUnloadPreviousSceneWithSpiralFX());
        }

        private IEnumerator CoUnloadPreviousSceneWithSpiralFX()
        {
            _spiralConfigSO.OnSpiralIn();
            yield return new WaitForSeconds(_spiralConfigSO.Duration);
            if (_currentLoadedScene != null)
            {
                if (_currentLoadedScene.SceneReference.OperationHandle.IsValid())
                    _currentLoadedScene.SceneReference.UnLoadScene();
#if UNITY_EDITOR
                else
                    UnloadSceneWhenStartFromEditor();
#endif
            }

            LoadNewSceneWithSpiralFX();
        }

        private void LoadNewSceneWithSpiralFX()
        {
            _sceneLoadingOperationHandle =
                _sceneToLoad.SceneReference.LoadSceneAsync(LoadSceneMode.Additive, true, 0);
            _sceneLoadingOperationHandle.Completed += NewSceneLoadedWithSpiralFX;
        }


        private void NewSceneLoadedWithSpiralFX(AsyncOperationHandle<SceneInstance> asyncOpSceneInstance)
        {
            _currentLoadedScene = _sceneToLoad;

            var scene = asyncOpSceneInstance.Result.Scene;
            SceneManager.SetActiveScene(scene);

            _isLoading = false;

            _spiralConfigSO.OnSpiralOut();
            _sceneLoaded.RaiseEvent();
        }
    }
}