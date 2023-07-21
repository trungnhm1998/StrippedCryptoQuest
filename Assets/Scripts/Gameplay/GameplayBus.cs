using CryptoQuest.Character.MonoBehaviours;
using UnityEngine;
using UnityEngine.Events;
using CryptoQuest.Gameplay.Battle.Core.Components;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;

namespace CryptoQuest.Gameplay
{
    public class GameplayBus : ScriptableObject
    {
        public HeroBehaviour Hero;
        public BattleTeam PlayerTeam;
        public AbilitySystemBehaviour MainSystem;

        public UnityAction HeroSpawned;

        public void RaiseHeroSpawnedEvent()
        {
            HeroSpawned?.Invoke();
        }
    }
}