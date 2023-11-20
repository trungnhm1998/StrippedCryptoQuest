using System;
using System.Collections;
using System.Collections.Generic;
using CommandTerminal;
using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.Battle.Components.EnemyComponents;
using CryptoQuest.Character.Enemy;
using UnityEngine;
using UnityEngine.AddressableAssets;
#if UNITY_EDITOR
#endif

namespace CryptoQuest.System.Cheat
{
    [RequireComponent(typeof(CastableCheats))]
    public class EnemySkillCheats : MonoBehaviour, ICheatInitializer
    {
        [SerializeField] private CastableCheats _castableCheat;

        private void OnValidate()
        {
            _castableCheat = GetComponent<CastableCheats>();
        }

        public Dictionary<string, AssetReferenceT<CastSkillAbility>> CastAbilityDict
            => _castableCheat.CastAbilityDict;

        public void InitCheats()
        {
            Terminal.Shell.AddCommand("add.skill.enemy", AddSkillToEnemy, 3, 3,
                "add.skill.enemy <skill_id> <skill-probability> <character_id>, add skill with id and probability to enemy\n" +
                "if enemy already has skill you can override that skill probability with this syntax");
            Terminal.Shell.AddCommand("get.skill.enemy", GetSkillFromEnemy, 1, 1,
                "get.skill.enemy <character_id>, get enemy skill infomation");
            Terminal.Shell.AddCommand("set.enemy.atttack.probability", SetNormalAttackEnemy, 2, 2,
                "set.enemy.atttack.probability <attack-probability> <character_id>, set normal attack probability of enemy");
        }

        private void AddSkillToEnemy(CommandArg[] args)
        {
            var abilityId = args[0].String;
            var probability = args[1].Float;
            var characterId = args[2].Int;

            if (!TryGetCharacter(characterId, out var character)) return;

            var enemySkillHolder = character.GetComponent<IEnemySkillHolder>();
            if (enemySkillHolder == null)
            {
                Debug.LogWarning($"{character.DisplayName} is not enemy");
                return;
            }

            StartCoroutine(LoadSkillCo(abilityId, skill =>
            {
                var existSkill = enemySkillHolder.Skills.Find(s => s.SkillDef == skill);
                if (existSkill.SkillDef != null)
                {
                    Debug.LogWarning($"Enemy {character.DisplayName} already has skill {skill.name}\n" +
                                     $"Skill will be remove and overrided");
                    enemySkillHolder.Skills.Remove(existSkill);
                }

                Debug.Log($"Added skill {skill.name} to enemy with probability {probability}");
                enemySkillHolder.Skills.Insert(0, new Skills()
                {
                    Probability = probability,
                    SkillDef = skill
                });
            }));
        }

        private void GetSkillFromEnemy(CommandArg[] args)
        {
            var characterId = args[0].Int;

            if (!TryGetCharacter(characterId, out var character)) return;


            var enemySkillHolder = character.GetComponent<IEnemySkillHolder>();
            if (enemySkillHolder == null)
            {
                Debug.LogWarning($"{character.DisplayName} is not enemy");
                return;
            }

            Debug.Log($"Enemy {character.DisplayName} skill information:");
            foreach (var skill in enemySkillHolder.Skills)
            {
                Debug.Log($"Skill: {skill.SkillDef.name}, probability {skill.Probability}");
            }
        }

        private IEnumerator LoadSkillCo(string abilityId, Action<CastSkillAbility> callback)
        {
            if (!CastAbilityDict.TryGetValue(abilityId, out var abilityAssetRef))
            {
                Debug.LogWarning($"Doesn't support ability [{abilityId}].");
                yield break;
            }

            CastSkillAbility ability = abilityAssetRef.Asset as CastSkillAbility;
            if (abilityAssetRef.Asset == null)
            {
                var handle = abilityAssetRef.LoadAssetAsync();
                yield return handle;
                ability = handle.Result;
            }

            callback?.Invoke(ability);
        }

        private void SetNormalAttackEnemy(CommandArg[] args)
        {
            var probability = args[0].Float;
            var characterId = args[1].Int;

            if (!TryGetCharacter(characterId, out var character)) return;
            var enemyCommandSelector = character.GetComponent<EnemyCommandSelector>();
            if (enemyCommandSelector == null)
            {
                Debug.LogWarning($"{character.DisplayName} is not enemy");
                return;
            }
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            enemyCommandSelector.Editor_SetNormalAttackProbability(probability);
            Debug.Log($"Set normal attack probability of {character.DisplayName} to {probability}");
#endif
        }

        private bool TryGetCharacter(int characterId, out Battle.Components.Character character)
        {
            character = CharacterCheats.Instance.GetCharacter(characterId);
            if (character == null)
            {
                Debug.LogWarning($"Cannot find character with instance id [{characterId}].");
                return false;
            }

            return true;
        }
    }
}