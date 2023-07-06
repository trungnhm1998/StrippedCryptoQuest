using System;
using System.Collections;
using System.Collections.Generic;
using IndiGames.Core.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
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

        [Header("Raised events")]
        [SerializeField] private StringEventChannelSO _onShowMapNameUI;

        private LocalizedString _mapNameLocalizedKey;

        private void OnEnable()
        {
            _sceneLoaded.EventRaised += OnSceneLoaded;
        }

        private void OnDisable()
        {
            _sceneLoaded.EventRaised -= OnSceneLoaded;
        }

        private void OnSceneLoaded()
        {
            if (String.IsNullOrEmpty(_sceneScriptableObject.localizedName)) return;
            LocalizationSettings.StringDatabase
                .GetLocalizedStringAsync(_tableReference, _sceneScriptableObject.localizedName)
                .Completed += result => _onShowMapNameUI.RaiseEvent(result.Result);
        }
    }
}