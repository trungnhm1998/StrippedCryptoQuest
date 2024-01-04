using CryptoQuest.Map;
using CryptoQuest.System;
using CryptoQuest.UI.SpiralFX;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
using IndiGames.Core.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using TinyMessenger;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.Gameplay.Manager
{
    public class EscapeAction : ActionBase
    {
        public UnityAction OnEscapeSucceeded;
        public UnityAction OnEscapeFailed;
    }

    public class EscapeAbilityHandler : MonoBehaviour
    {
        [SerializeField] private SceneScriptableObject _destinationScene;
        [SerializeField] private PathStorageSO _pathStorageSo;
        [SerializeField] private LoadSceneEventChannelSO _loadMapEventChannel;
        [SerializeField] private VoidEventChannelSO _onSceneLoadedEventChannel;
        [SerializeField] private SpiralConfigSO _spiralConfig;
        [SerializeField] private EscapeRouteMappingSO _escapeRouteMapping;
        [SerializeField] private VoidEventChannelSO _closeMainMenuEventChannel;
        [SerializeField] private SceneManagerSO _sceneManagerSO;
        private TinyMessageSubscriptionToken _token;

        private void OnEnable()
        {
            _token = ActionDispatcher.Bind<EscapeAction>(HandleEscape);
        }

        private void OnDisable()
        {
            ActionDispatcher.Unbind(_token);
        }

        private void HandleEscape(EscapeAction escapeAction)
        {
            SceneScriptableObject currentScene = _sceneManagerSO.CurrentScene;

            if (_escapeRouteMapping.MapToEscapePathDictionary.TryGetValue(currentScene, out MapPathSO escapePath))
            {
                OnEscapeSucceeded(escapePath);
                escapeAction.OnEscapeSucceeded?.Invoke();
            }
            else
            {
                Debug.LogWarning($"No escape route found for scene {currentScene.name}");
                escapeAction.OnEscapeFailed?.Invoke();
            }
        }

        private void OnEscapeSucceeded(MapPathSO escapePath)
        {
            _closeMainMenuEventChannel.RaiseEvent();
            _pathStorageSo.LastTakenPath = escapePath;
            _spiralConfig.DoneSpiralIn += TriggerEscape;
            _spiralConfig.DoneSpiralOut += FinishTransition;
            _onSceneLoadedEventChannel.EventRaised += _spiralConfig.HideSpiral;
            _spiralConfig.ShowSpiral();
        }

        private void TriggerEscape()
        {
            _loadMapEventChannel.RequestLoad(_destinationScene);
        }

        private void FinishTransition()
        {
            _spiralConfig.DoneSpiralIn -= TriggerEscape;
            _spiralConfig.DoneSpiralOut -= FinishTransition;
            _onSceneLoadedEventChannel.EventRaised -= _spiralConfig.HideSpiral;
            _spiralConfig.Color = Color.black;
        }
    }
}