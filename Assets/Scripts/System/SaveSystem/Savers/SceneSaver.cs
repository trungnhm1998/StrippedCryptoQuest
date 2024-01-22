using System;
using IndiGames.Core.SceneManagementSystem.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.System.SaveSystem.Savers
{
    [Serializable]
    public class SceneSaver : SaverBase
    {
        public static readonly string Key = "Scene";

        // This event only for loading location so we don't need to check the scene type
        [SerializeField] private LoadSceneEventChannelSO _loadMap;

        public override void RegistEvents() => _loadMap.LoadingRequested += SaveCurrentScene;
        public override void UnregistEvents() => _loadMap.LoadingRequested -= SaveCurrentScene;

        private void SaveCurrentScene(SceneScriptableObject sceneToLoad)
        {
            _saveSystem.SaveData[Key] = sceneToLoad.Guid;
            _saveHandler.Save();
        }
    }
}