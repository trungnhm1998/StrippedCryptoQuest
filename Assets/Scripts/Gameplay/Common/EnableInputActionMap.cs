using CryptoQuest.Input;
using UnityEngine;

namespace CryptoQuest.Gameplay.Common
{
    public class EnableInputActionMap : MonoBehaviour
    {
        [SerializeField] private InputMediatorSO _inputMediator;
        [SerializeField] private GameStateSO _gameState;

        private void Start()
        {
            _gameState.UpdateGameState(EGameState.Field);
            _inputMediator.EnableMapGameplayInput();
        }
    }
}