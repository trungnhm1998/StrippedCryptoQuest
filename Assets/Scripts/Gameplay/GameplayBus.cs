using CryptoQuest.Character.MonoBehaviours;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.Gameplay
{
    public class GameplayBus : ScriptableObject
    {
        [SerializeField] private GameStateSO _gameState;
        public HeroBehaviour Hero;

        public UnityAction HeroSpawned;

        public void RaiseHeroSpawnedEvent()
        {
            _gameState.UpdateGameState(EGameState.Field);
            HeroSpawned?.Invoke();
        }
    }
}