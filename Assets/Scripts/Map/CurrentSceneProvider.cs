using IndiGames.Core.SceneManagementSystem.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Map
{
    public class CurrentSceneProvider : MonoBehaviour
    {
        [SerializeField] private LoadSceneEventChannelSO _coldBootEvent;
        [SerializeField] private LoadSceneEventChannelSO _loadSceneEvent;
        [SerializeField] private SceneManagerSO _sceneManagerSO;

        private void OnEnable()
        {
            _loadSceneEvent.LoadingRequested += OnLoadScene;
            _coldBootEvent.LoadingRequested += OnLoadScene;
        }

        private void OnDisable()
        {
            _loadSceneEvent.LoadingRequested -= OnLoadScene;
            _coldBootEvent.LoadingRequested -= OnLoadScene;
        }

        private void OnLoadScene(SceneScriptableObject scene)
        {
            _sceneManagerSO.CurrentScene = scene;
        }
    }
}