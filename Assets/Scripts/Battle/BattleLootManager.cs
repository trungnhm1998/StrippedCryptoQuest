using System.Collections.Generic;
using System.Linq;
using CryptoQuest.Battle.Components;
using CryptoQuest.Gameplay.Loot;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.System;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Battle
{
    public class BattleLootManager : MonoBehaviour
    {
        [SerializeField] private BattleContext _context;
        [SerializeField] private AttributeScriptableObject _dropRateAttribute;

        private IPartyController _partyController;
        private ILootMerger _lootMerger;

        private void Awake()
        {
            _lootMerger = new LootMerger(); // TODO: Inject instead
        }

        public List<LootInfo> GetDroppedLoots()
        {
            var currentBuff = GetDropRateBuffFromParty();
            Debug.Log($"BattleLootManager:: Buff drop rate {currentBuff}");

            var loots = new List<LootInfo>();
            foreach (var enemy in _context.Enemies.Where(enemy => enemy.Spec.IsValid()))
                loots.AddRange(GetDropFromEnemy(enemy, currentBuff));

            return _lootMerger.Merge(loots);
        }

        private float GetDropRateBuffFromParty()
        {
            _partyController ??= ServiceProvider.GetService<IPartyController>();
            float buff = 0f;

            foreach (var member in _partyController.OrderedAliveMembers)
            {
                if (!member.AttributeSystem.TryGetAttributeValue(_dropRateAttribute, out var buffValue))
                    continue;
                buff += buffValue.CurrentValue;
            }

            return buff;
        }

        private IEnumerable<LootInfo> GetDropFromEnemy(EnemyBehaviour enemy, float dropBuff = 0)
        {
            var drops = enemy.GetComponent<IDropsProvider>().GetDrops();
            foreach (var drop in drops)
            {
                var randomChance = Random.Range(0f, 1f);
                var dropChance = drop.Chance + dropBuff;
                Debug.Log($"BattleLootManager:: Drop chance {dropChance} random {randomChance}");
                if (randomChance > dropChance) continue;
                yield return drop.GetLoot();
            }
        }
    }
}