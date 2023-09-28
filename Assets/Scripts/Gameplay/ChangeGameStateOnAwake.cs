using UnityEngine;

namespace CryptoQuest.Gameplay
{
    public class ChangeGameStateOnAwake : MonoBehaviour
    {
        [SerializeField] private EGameState _gameStateType;
        [SerializeField] private GameStateSO _gameStateSO;
        private void Awake() => _gameStateSO.UpdateGameState(_gameStateType);
    }
}