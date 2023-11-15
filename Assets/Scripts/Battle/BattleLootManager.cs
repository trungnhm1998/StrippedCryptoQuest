using System.Collections.Generic;
using System.Linq;
using CryptoQuest.Battle.Components;
using CryptoQuest.Gameplay.Loot;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.Gameplay.Reward;
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

        private float GetDropRateBuffFromParty()
        {
            var party = ServiceProvider.GetService<IPartyController>();
            float buff = 0f;

            foreach (var member in party.OrderedAliveMembers)
            {
                if (!member.AttributeSystem.TryGetAttributeValue(_dropRateAttribute, out var buffValue))
                    continue;
                buff += buffValue.CurrentValue;
            }

            return buff;
        }

        public List<LootInfo> GetDropLoots()
        {
            var currentBuff = GetDropRateBuffFromParty();
            Debug.Log($"BattleLootManager:: Buff drop rate {currentBuff}");

            var loots = new List<LootInfo>();
            foreach (var enemy in _context.Enemies.Where(enemy => enemy.Spec.IsValid()))
            {
                loots.AddRange(GetDropFromEnemy(enemy, currentBuff));
            }

            loots = RewardManager.CloneAndMergeLoots(loots);
            return loots;
        }

        private IEnumerable<LootInfo> GetDropFromEnemy(EnemyBehaviour enemy, float dropBuff = 0)
        {
            foreach (var drop in enemy.Def.Drops)
            {
                var randomChance = UnityEngine.Random.Range(0f, 1f);
                var dropChance = drop.Chance + dropBuff;
                if (randomChance > dropChance) continue;
                yield return drop.CreateLoot();
            }
        }
    }
}