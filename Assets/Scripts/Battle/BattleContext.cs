using System.Collections.Generic;
using System.Linq;
using CryptoQuest.AbilitySystem;
using CryptoQuest.AbilitySystem.Attributes;
using CryptoQuest.Battle.Components;
using CryptoQuest.Gameplay.Encounter;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.System;
using IndiGames.Core.Common;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Battle
{
    /// <summary>
    /// Holds the context of current battle, all heroes and enemies
    /// </summary>
    public class BattleContext : MonoBehaviour
    {
        [SerializeField] private BattleBus _battleBus;
        [SerializeField] private EnemyPartyManager _enemyPartyManager;
        [SerializeField] private VoidEventChannelSO _sceneLoadedEvent; // Awake only work if we start from correct flow
        public List<EnemyBehaviour> Enemies => _enemyPartyManager.Enemies;
        public List<EnemyGroup> EnemyGroups => _enemyPartyManager.EnemyGroups;
        private IPartyController _party;
        public IPartyController PlayerParty => _party;
        public Battlefield CurrentBattlefield => _battleBus.CurrentBattlefield;

        public List<EnemyBehaviour> AliveEnemies { get; private set; } = new();
        public List<HeroBehaviour> AliveHeroes { get; private set; } = new();


        private void Awake()
        {
            _battleBus.CurrentBattleContext = this;
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
                where enemy.IsValidAndAlive()
                select TryGetCharacter(enemy)).ToList();
            aliveCharacters.AddRange(from slot in _party.Slots
                where slot.HeroBehaviour.IsValidAndAlive()
                select TryGetCharacter(slot.HeroBehaviour));

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

        public bool IsAllEnemiesDead => AliveEnemies.Count <= 0;
        public bool IsAllHeroesDead => AliveHeroes.Count <= 0;

        /// <summary>
        /// Use this each turn to update and cache characters alive status
        /// </summary>
        public void UpdateBattleContext()
        {
            AliveEnemies = Enemies.Where(enemy => enemy.IsValidAndAlive()).ToList();
            AliveHeroes = PlayerParty.Slots
                .Where(slot => slot.HeroBehaviour.IsValidAndAlive() && !slot.HeroBehaviour.HasTag(TagsDef.DisableActionInfinity))
                .Select(slot => slot.HeroBehaviour).ToList();
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