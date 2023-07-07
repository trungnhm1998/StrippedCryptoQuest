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
        [SerializeField] private SceneScriptableObject _sceneScriptableObject;
        [SerializeField] private TableReference _tableReference;

        [Header("Listened events")]
        [SerializeField] private VoidEventChannelSO _sceneLoaded;

        [SerializeField] private LoadSceneEventChannelSO _loadMapEvent;

        [Header("Raised events")]
        [SerializeField] private StringEventChannelSO _onShowMapNameUI;

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
            if (String.IsNullOrEmpty(_sceneScriptableObject.localizedName))
            {
                _onShowMapNameUI.RaiseEvent(_sceneScriptableObject.name);
                return;
            }
            LocalizationSettings.StringDatabase
                .GetLocalizedStringAsync(_tableReference, _sceneScriptableObject.localizedName)
                .Completed += result => _onShowMapNameUI.RaiseEvent(result.Result);
        }

        private void OnLoadNewScene(SceneScriptableObject scene)
        {
            _onHideMapNameUI.RaiseEvent();
        }
    }
}