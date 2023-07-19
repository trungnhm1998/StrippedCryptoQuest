using CryptoQuest.Character.MonoBehaviours;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.Gameplay
{
    public class GameplayBus : ScriptableObject
    {
        public HeroBehaviour Hero;

        public UnityAction HeroSpawned;

        public void RaiseHeroSpawnedEvent()
        {
            HeroSpawned?.Invoke();
        }
    }
}