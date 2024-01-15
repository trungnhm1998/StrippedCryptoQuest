using CryptoQuest.Gameplay;
using CryptoQuest.Input;
using UnityEngine;

namespace CryptoQuest.UI.Dialogs
{
    public class GameStateDialogInputManager : MonoBehaviour
    {
        [SerializeField] private GameStateSO _gameState;
        [SerializeField] private InputMediatorSO _input;

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
            if (newState == EGameState.Dialogue)
            {
                _input.EnableDialogueInput();
                return;
            }
        }
    }
}