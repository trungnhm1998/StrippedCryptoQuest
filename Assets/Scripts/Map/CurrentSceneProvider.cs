using System.Collections;
using CryptoQuest.System;
using IndiGames.Core.Common;
using IndiGames.Core.SceneManagementSystem.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CryptoQuest.Map
{
    public class CurrentSceneProvider : MonoBehaviour
    {
        [SerializeField] private LoadSceneEventChannelSO _loadSceneEvent;
        [SerializeField] private SceneManagerSO _sceneManagerSO;

        private void OnEnable()
        {
            _loadSceneEvent.LoadingRequested += OnLoadScene;
        }

        private void OnDisable()
        {
            _loadSceneEvent.LoadingRequested -= OnLoadScene;
        }

        private void OnLoadScene(SceneScriptableObject scene)
        {
            _sceneManagerSO.CurrentScene = scene;
        }
    }
}