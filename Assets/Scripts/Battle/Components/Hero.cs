using System;
using CryptoQuest.Gameplay.Character;
using UnityEngine;

namespace CryptoQuest.Battle.Components
{
    public interface IHero
    {
        GameObject GameObject { get; }
        void Init(CharacterSpec character);
    }

    public class Hero : MonoBehaviour, IHero
    {
        private CharacterSpec _characterSpec;
        public GameObject GameObject => gameObject;
        private ICharacter _characterComponent;

        private void Awake()
        {
            _characterComponent = GetComponent<ICharacter>();
        }

        public void Init(CharacterSpec character)
        {
            if (character.IsValid() == false) return;
            _characterSpec = character;
            
            var statsInitializer = GetComponent<IStatsInitializer>();
            _characterComponent.Init(character.Element);
        }
    }
}