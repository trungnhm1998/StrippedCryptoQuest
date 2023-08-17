using CryptoQuest.Character.MonoBehaviours;
using UnityEngine;
using UnityEngine.Events;
using CryptoQuest.Gameplay.Battle.Core.Components;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;

namespace CryptoQuest.Gameplay
{
    public class GameplayBus : ScriptableObject
    {
        [SerializeField] private GameStateSO _gameState;
        public HeroBehaviour Hero;
        public AbilitySystemBehaviour MainSystem;

        public UnityAction HeroSpawned;

        public void RaiseHeroSpawnedEvent()
        {
            _gameState.UpdateGameState(EGameState.Field);
            HeroSpawned?.Invoke();
        }
    }
}