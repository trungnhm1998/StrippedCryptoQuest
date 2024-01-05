using CryptoQuest.Map;
using CryptoQuest.System.SaveSystem.Sagas;
using IndiGames.Core.Events;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.System.SaveSystem
{
    public class RegisterLandingScene : MonoBehaviour
    {
        private static readonly string Key = "Scene";
        [SerializeField] private SaveSystemSO _saveSystem;
        [SerializeField] private PathStorageSO _pathStorage;
        [SerializeField] private SceneScriptableObject _landingScene;
        [SerializeField] private MapPathSO _landingMapPath;

        public void Register()
        {
            _saveSystem.SaveData[Key] = _landingScene.Guid;
            _pathStorage.LastTakenPath = _landingMapPath;
            _saveSystem.Save();
            ActionDispatcher.Dispatch(new UploadProfileAction());
        }
    }
}