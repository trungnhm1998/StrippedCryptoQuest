using System.Collections;
using CryptoQuest.System;
using IndiGames.Core.Common;
using IndiGames.Core.SceneManagementSystem.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CryptoQuest.Map
{
    public interface ICurrentSceneProvider
    {
        SceneScriptableObject CurrentScene { get; }
    }

    public class CurrentSceneProvider : MonoBehaviour, ICurrentSceneProvider
    {
        public SceneScriptableObject CurrentScene { get; private set; }
        [SerializeField] private LoadSceneEventChannelSO _loadSceneEvent;

        private void Awake()
        {
            ServiceProvider.Provide<ICurrentSceneProvider>(this);
        }

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
            CurrentScene = scene;
        }
    }
}