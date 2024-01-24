using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CryptoQuest.Gameplay;
using IndiGames.Core.SceneManagementSystem.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using IndiGames.Core.UI;
using UnityEngine;

namespace CryptoQuest.System.TransitionSystem
{
    public class LoadingStateController : MonoBehaviour
    {
        [SerializeField] private GameStateSO _gameState;
        [SerializeField] private LoadSceneEventChannelSO _loadSceneEvent;
        [SerializeField] private FadeConfigSO _fade;
        private bool _isLoading = false;
        private List<EGameState> _cachedStates = new();

        private void OnEnable()
        {
            _gameState.Changed += OnChanged;
            _loadSceneEvent.LoadingRequested += OnLoadingRequested;
            _fade.FadeOutComplete += OnTransitionOut;
        }

        private void OnDisable()
        {
            _gameState.Changed -= OnChanged;
            _loadSceneEvent.LoadingRequested -= OnLoadingRequested;
            _fade.FadeOutComplete -= OnTransitionOut;
        }

        private void OnLoadingRequested(SceneScriptableObject arg0)
        {
            _gameState.UpdateGameState(EGameState.Loading);
            _isLoading = true;
        }

        private void OnChanged(EGameState newState)
        {
            if (!_isLoading) return;
            if (newState != EGameState.Loading)
                _cachedStates.Add(newState);
            StartCoroutine(CoKeepLoading());
        }

        private IEnumerator CoKeepLoading()
        {
            yield return null;
            while (_isLoading)
            {
                _gameState.UpdateGameState(EGameState.Loading);
                yield return null;
            }
        }


        private void OnTransitionOut()
        {
            if (!_isLoading) return;
            UpdateState();
        }

        private void UpdateState()
        {
            _isLoading = false;
            if (_cachedStates.Count == 0) return;
            var currentState = _cachedStates.Last();
            for (int i = _cachedStates.Count - 2; i >= 0; i--)
            {
                if (_cachedStates[i] == currentState) continue;
                _gameState.UpdateGameState(_cachedStates[i]);
                break;
            }

            _gameState.UpdateGameState(currentState);
            _cachedStates.Clear();
        }
    }
}