using CryptoQuest.System;
using IndiGames.Core.EditorTools;
using IndiGames.Core.SceneManagementSystem.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest
{
    public interface ICheckPointProvider
    {
        void SaveCheckPoint(Vector3 position);
    }
    public class CheckPointProvider : MonoBehaviour, ICheckPointProvider
    {
        private SceneScriptableObject _lastCheckPointScene;
        private SceneScriptableObject _currentScene;
        private Vector3 _lastCheckPointPosition;

        [Header("Listening on")]
        [SerializeField] private LoadSceneEventChannelSO _loadSceneEvent;

        private void Awake()
        {
            ServiceProvider.Provide<ICheckPointProvider>(this);
            _loadSceneEvent.LoadingRequested += LoadNextScene;
        }
        private void OnDestroy()
        {
            _loadSceneEvent.LoadingRequested -= LoadNextScene;
        }

#if UNITY_EDITOR
        private void Start()
        {
            var editorColdBoot = FindObjectOfType<EditorColdBoot>();
            _currentScene = editorColdBoot.ThisScene;
        }
#endif
        private void LoadNextScene(SceneScriptableObject nextScene)
        {
            _currentScene = nextScene;
            Debug.Log("Checkpoint: Set current scene " + nextScene.name);
        }    

        public void SaveCheckPoint(Vector3 position)
        {
            _lastCheckPointScene = _currentScene;
            _lastCheckPointPosition = position;
            Debug.Log($"Checkpoint: Save checkpoint at {_lastCheckPointScene.name} at {position.ToString()}");
        }    
    }
}
