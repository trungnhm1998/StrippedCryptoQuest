using System.Collections.Generic;
using System.Linq;
using CryptoQuest.Battle.Components;
using CryptoQuest.Character.Attributes;
using CryptoQuest.Character.Tag;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.System;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Battle
{
    public class BattleContext : MonoBehaviour
    {
        [SerializeField] private EnemyPartyManager _enemyPartyManager;
        [SerializeField] private VoidEventChannelSO _sceneLoadedEvent; // Awake only work if we start from correct flow
        public List<EnemyBehaviour> Enemies => _enemyPartyManager.Enemies;
        private IPartyController _party;
        public IPartyController PlayerParty => _party;

        private void Awake()
        {
            _sceneLoadedEvent.EventRaised += OnSceneLoaded;
        }

        private void OnDestroy()
        {
            _sceneLoadedEvent.EventRaised -= OnSceneLoaded;
        }

        private void OnSceneLoaded()
        {
            // TODO: Use SO as a cross scene IPartyController that can be Serialized, either way need to wait for scene loaded event if start from editor
            _party = ServiceProvider.GetService<IPartyController>();
        }

        /// <summary>
        /// Get up to date alive characters.
        /// </summary>
        /// <returns></returns>
        public List<Components.Character> GetAliveCharacters()
        {
            var aliveCharacters = (from enemy in _enemyPartyManager.Enemies
                where enemy.IsValid() && !enemy.HasTag(TagsDef.Dead)
                select TryGetCharacter(enemy)).ToList();
            aliveCharacters.AddRange(from hero in _party.Slots
                where hero.IsValid() && !hero.HeroBehaviour.HasTag(TagsDef.Dead)
                select TryGetCharacter(hero.HeroBehaviour));

            return aliveCharacters;
        }

        public List<Components.Character> GetSortedAliveCharacterBasedOnAgi()
        {
            var aliveCharacters = GetAliveCharacters();
            return aliveCharacters.OrderByDescending(character =>
            {
                character.AttributeSystem.TryGetAttributeValue(AttributeSets.Agility, out var agi);
                return agi.CurrentValue;
            }).ToList();
        }

        private static Components.Character TryGetCharacter(Component character)
        {
            if (character.TryGetComponent<Components.Character>(out var characterComponent))
            {
                if (characterComponent.HasTag(TagsDef.Dead)) return null;
                return characterComponent;
            }

            Debug.LogWarning($"Failed to get {nameof(Components.Character)} component from {character.name}.");
            return null;
        }
    }
}