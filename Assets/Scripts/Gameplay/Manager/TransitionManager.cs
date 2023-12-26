using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Input;
using CryptoQuest.Map;
using CryptoQuest.UI.SpiralFX;
using IndiGames.Core.Events;
using IndiGames.Core.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using IndiGames.Core.UI;
using TinyMessenger;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CryptoQuest.Gameplay.Manager
{
    public class TriggerTransitionAction : ActionBase
    {
        public MapPathSO Path;

        public TriggerTransitionAction(MapPathSO path)
        {
            Path = path;
        }
    }

    public class DoneTransitionInAction : ActionBase { }

    public class DoneTransitionOutAction : ActionBase { }

    public class TransitionManager : MonoBehaviour
    {
        [SerializeField] private SceneScriptableObject _destinationScene;
        [SerializeField] private SpiralConfigSO _spiralConfig;
        [SerializeField] private FadeConfigSO _fadeConfig;
        [SerializeField] private PathStorageSO _pathStorageSo;
        [SerializeField] private LoadSceneEventChannelSO _loadMapEventChannel;
        [SerializeField] private VoidEventChannelSO _onSceneLoadedEventChannel;
        [SerializeField] private Color _transitionColor = Color.black;
        [SerializeField] private Color _resetColor = Color.black;
        [SerializeField] private GameplayBus _gameplayBus;
        [SerializeField] private InputMediatorSO _inputMediatorSO;
        private TinyMessageSubscriptionToken _requestTransitionToken;
        private MapPathSO _currentSelectedPath;
        private readonly List<GoFrom> _cachedDestinations = new();

        private void OnEnable()
        {
            _requestTransitionToken = ActionDispatcher.Bind<TriggerTransitionAction>(HandleTransition);
            _onSceneLoadedEventChannel.EventRaised += ResetCache;
        }

        private void ResetCache()
        {
            _cachedDestinations.Clear();
        }

        private void OnDisable()
        {
            ActionDispatcher.Unbind(_requestTransitionToken);
            _onSceneLoadedEventChannel.EventRaised -= ResetCache;
            _cachedDestinations.Clear();
        }

        private void HandleTransition(TriggerTransitionAction ctx)
        {
            TriggerTransition(ctx.Path);
            _currentSelectedPath = ctx.Path;
        }


        private void TriggerTransition(MapPathSO path)
        {
            _pathStorageSo.LastTakenPath = path;
            _spiralConfig.DoneSpiralIn += TriggerLoadScene;
            _spiralConfig.DoneSpiralOut += FinishTransition;
            _onSceneLoadedEventChannel.EventRaised += _spiralConfig.HideSpiral;
            _spiralConfig.Color = _transitionColor;
            _fadeConfig.FadeInColor = _transitionColor;
            _fadeConfig.FadeOutColor = _transitionColor;
            _fadeConfig.FadeOutColor.a = 0;
            _spiralConfig.ShowSpiral();
        }

        private void TriggerLoadScene()
        {
            if (SceneManager.GetSceneByName("WorldMap").isLoaded)
            {
                MoveHeroToLocationEntrance(_currentSelectedPath);
            }
            else
            {
                _loadMapEventChannel.RequestLoad(_destinationScene);
                _spiralConfig.DoneSpiralIn -= TriggerLoadScene;
                _cachedDestinations.Clear();
            }
        }

        private void MoveHeroToLocationEntrance(MapPathSO path)
        {
            if (_cachedDestinations.Count == 0)
            {
                List<GoFrom> destinations = new List<GoFrom>();
                destinations.AddRange(FindObjectsOfType<GoFrom>());
                foreach (var destination in destinations)
                {
                    if (destination.MapPath.name.ToLower().Contains("ocarina") ||
                        destination.MapPath.name.ToLower().Contains("transfer"))
                        _cachedDestinations.Add(destination);
                }
            }

            foreach (GoFrom destination in _cachedDestinations)
            {
                if (path != destination.MapPath) continue;
                _gameplayBus.Hero.transform.position = destination.transform.position;
                StartCoroutine(CoHideSpiralAndEnableMapInput());
                return;
            }
        }

        private IEnumerator CoHideSpiralAndEnableMapInput()
        {
            _spiralConfig.HideSpiral();
            yield return new WaitForSeconds(_spiralConfig.Duration);
            _inputMediatorSO.EnableMapGameplayInput();
        }

        private void FinishTransition()
        {
            _spiralConfig.DoneSpiralIn -= TriggerLoadScene;
            _spiralConfig.DoneSpiralOut -= FinishTransition;
            _onSceneLoadedEventChannel.EventRaised -= _spiralConfig.HideSpiral;
            _spiralConfig.Color = _resetColor;
            _fadeConfig.FadeInColor = _resetColor;
            _fadeConfig.FadeOutColor = _resetColor;
            _fadeConfig.FadeOutColor.a = 0;
            _currentSelectedPath = null;
        }
    }
}