using CryptoQuest.System.SaveSystem;
using IndiGames.Core.SceneManagementSystem;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CryptoQuest.System.SceneManagement
{
    public class SceneLoaderHandler : LinearGameSceneLoader
    {
        [SerializeField] private SceneLoaderBus _sceneLoadBus;
        [SerializeField] private SaveSystemSO _saveSystemSO;

        protected override void OnEnable()
        {
            ServiceProvider.Provide<ISaveSystem>(_saveSystemSO);
            _sceneLoadBus.SceneLoader = this;
            base.OnEnable();
        }

        protected override void OnSceneLoaded(Scene scene)
        {
            base.OnSceneLoaded(scene);
            if (SceneToLoad.SceneType == SceneScriptableObject.Type.Location)
            {
                if (_saveSystemSO != null)
                {
                    if (!_saveSystemSO.IsLoadingSaveGame())
                    {
                        Debug.Log("ISceneLoaderHandler, OnSceneLoaded, save scene...");
                        _saveSystemSO.SaveScene(SceneToLoad);
                    }
                    Debug.Log("ISceneLoaderHandler, OnSceneLoaded, loaded scene...");
                    _saveSystemSO.OnSceneLoaded();
                }
            }
        }
    }
}