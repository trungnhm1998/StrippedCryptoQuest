using CryptoQuest.Gameplay;
using UnityEngine;

namespace CryptoQuest.Merchant
{
    public class GameStateMerchantInputManager : MonoBehaviour
    {
        [SerializeField] private GameStateSO _gameState;
        [SerializeField] private MerchantInput _input;

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
            if (newState == EGameState.Merchant)
            {
                _input.EnableInput();
                return;
            }

            _input.DisableInput();
        }
    }
}