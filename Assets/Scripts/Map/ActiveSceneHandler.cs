using IndiGames.Core.SceneManagementSystem.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Map
{
    public class ActiveSceneHandler : MonoBehaviour
    {
        [SerializeField] private LoadSceneEventChannelSO _loadMapEventChannelSO;
        [SerializeField] private ActiveSceneSO _activeSceneSO;

        private void OnEnable()
        {
            _loadMapEventChannelSO.LoadingRequested += SaveCurrentActiveMap;
        }

        private void OnDisable()
        {
            _loadMapEventChannelSO.LoadingRequested -= SaveCurrentActiveMap;
        }

        private void SaveCurrentActiveMap(SceneScriptableObject sceneSo)
        {
            if (sceneSo.SceneType != SceneScriptableObject.Type.Location) return;
            _activeSceneSO.CurrentActiveMapScene = sceneSo;
        }
    }
}