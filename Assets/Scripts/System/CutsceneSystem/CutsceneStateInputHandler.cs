using CryptoQuest.Gameplay;
using CryptoQuest.Input;
using CryptoQuest.System.CutsceneSystem;
using UnityEngine;

namespace CryptoQuest.System
{
    public class CutsceneStateInputHandler : MonoBehaviour
    {
        [SerializeField] private GameStateSO _gameState;
        [SerializeField] private CutsceneInput _input;

        private void OnEnable()
        {
            _gameState.Changed += OnChanged;
        }

        private void OnDisable()
        {
            _gameState.Changed -= OnChanged;
        }

        private void OnChanged(EGameState newState)
        {
            if (newState != EGameState.Cutscene) return;
            _input.EnableInput();
        }
    }
}