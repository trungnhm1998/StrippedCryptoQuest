using System;
using System.Collections;
using System.Collections.Generic;
using IndiGames.Core.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;

namespace CryptoQuest
{
    public class MapStorageController : MonoBehaviour
    {
        public MapStorageSO mapStorageSo;
        public LoadSceneEventChannelSO requestloadMapEvent;
        public VoidEventChannelSO mapLoadedEvent;
        private SceneScriptableObject _currentRequestedMap;

        private void OnEnable()
        {
            requestloadMapEvent.LoadingRequested += OnLoadMapRequested;
            mapLoadedEvent.EventRaised += OnMapLoaded;
        }

        private void OnDisable()
        {
            requestloadMapEvent.LoadingRequested -= OnLoadMapRequested;
            mapLoadedEvent.EventRaised -= OnMapLoaded;
        }

        public void OnLoadMapRequested(SceneScriptableObject sceneSO)
        {
            if (sceneSO.SceneType != SceneScriptableObject.Type.Location) return;
            _currentRequestedMap = sceneSO;
        }

        public void OnMapLoaded()
        {
            if (_currentRequestedMap == null) return;
            mapStorageSo.currentMapScene = _currentRequestedMap;
        }
    }
}