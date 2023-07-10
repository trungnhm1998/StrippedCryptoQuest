using System;
using System.Collections;
using System.Collections.Generic;
using IndiGames.Core.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using UnityEditor;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using UnityEngine.Serialization;
using UnityEngine.SocialPlatforms;

namespace CryptoQuest
{
    public class MapNameController : MonoBehaviour
    {
        [SerializeField] private LocalizedString _mapNameKey;

        [Header("Listened events")]
        [SerializeField] private VoidEventChannelSO _sceneLoaded;

        [SerializeField] private LoadSceneEventChannelSO _loadMapEvent;

        [Header("Raised events")]
        [SerializeField] private LocalizedStringEventChannelSO _onShowMapNameUI;

        [SerializeField] private VoidEventChannelSO _onHideMapNameUI;

        private LocalizedString _mapNameLocalizedKey;

        private void OnEnable()
        {
            _sceneLoaded.EventRaised += OnSceneLoaded;
            _loadMapEvent.LoadingRequested += OnLoadNewScene;
        }

        private void OnDisable()
        {
            _sceneLoaded.EventRaised -= OnSceneLoaded;
            _loadMapEvent.LoadingRequested -= OnLoadNewScene;
        }

        private void OnSceneLoaded()
        {
            if (_mapNameKey.IsEmpty) return;

            _onShowMapNameUI.RaiseEvent(_mapNameKey);
        }

        private void OnLoadNewScene(SceneScriptableObject scene)
        {
            _onHideMapNameUI.RaiseEvent();
        }
    }
}