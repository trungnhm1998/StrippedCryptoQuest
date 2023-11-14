using CryptoQuest.SaveSystem;
using IndiGames.Core.SceneManagementSystem.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.System.SaveSystem.Savers
{
    public class SceneSaver : MonoBehaviour
    {
        public static readonly string Key = "Scene";
        [SerializeField] private SaveSystemSO _saveSystem;

        // This event only for loading location so we don't need to check the scene type
        [SerializeField] private LoadSceneEventChannelSO _loadMap;

        private void OnEnable() => _loadMap.LoadingRequested += SaveCurrentScene;

        private void OnDisable() => _loadMap.LoadingRequested -= SaveCurrentScene;

        private void SaveCurrentScene(SceneScriptableObject sceneToLoad)
        {
            _saveSystem.SaveData[Key] = sceneToLoad.Guid;
            _saveSystem.Save();
        }
    }
}