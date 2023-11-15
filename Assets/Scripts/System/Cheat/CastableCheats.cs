using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CommandTerminal;
using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.Battle.Components;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CryptoQuest.System.Cheat
{
    public class CastableCheats : MonoBehaviour, ICheatInitializer
    {
        [SerializeField] private List<Castables> _castables;
        private readonly Dictionary<int, AssetReferenceT<CastSkillAbility>> _castAbilityDict = new();
        private readonly Dictionary<int, Dictionary<int, GameplayAbilitySpec>> _castAbilitySpecDict = new();

        [Serializable]
        private struct Castables
        {
            public int CastableId;
            public AssetReferenceT<CastSkillAbility> Ability;
        }
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_castables.Count != 0) return;
            var abilityGuids = AssetDatabase.FindAssets("t:CastSkillAbility");
            foreach (var guid in abilityGuids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var cast = AssetDatabase.LoadAssetAtPath<CastSkillAbility>(path);
                if (cast == null) continue;
                int id = cast.Context.SkillInfo.Id;
                if (id is >= 3000 or < 0) continue;

                Castables castables = new Castables()
                {
                    CastableId = id,
                    Ability = new AssetReferenceT<CastSkillAbility>(guid)
                };
                _castables.Add(castables);
            }
        }
#endif
        private void Awake()
        {
            foreach (var item in _castables)
            {
                _castAbilityDict.TryAdd(item.CastableId, item.Ability);
            }
        }

        public void InitCheats()
        {
            Terminal.Shell.AddCommand("add.cast", AddAbilityToCharacter, 2, 2,
                "add.cast <cast-able_id> <character_index>, add cast-able ability with id to character in party index");
            Terminal.Shell.AddCommand("remove.cast", RemoveCastAbilityFromCharacter, 2, 2,
                "remove.cast <cast-able_id> <character_index>, remove cast-able with id from character in party index");
        }

        private void AddAbilityToCharacter(CommandArg[] args)
        {
            var abilityIds = ExtractAbilityIds(args);
            var characterId = args[1].Int;
            foreach (var abilityId in abilityIds)
                StartCoroutine(LoadThenAddAbilityToCharacterCo(characterId, abilityId));
        }

        private static int[] ExtractAbilityIds(CommandArg[] args)
        {
            var strAbilityIds = args[0].String.Split(',');
            var abilityIds = new int[strAbilityIds.Length];
            for (var index = 0; index < strAbilityIds.Length; index++)
            {
                var strId = strAbilityIds[index];
                abilityIds[index] = int.Parse(strId);
            }

            return abilityIds;
        }

        private IEnumerator LoadThenAddAbilityToCharacterCo(int characterId, int abilityId)
        {
            var character = CharacterCheats.Instance.GetCharacter(characterId);
            var characterSkillHolder = character.GetComponent<HeroSkills>();
            if (character == null)
            {
                Debug.LogWarning($"Cannot find character with instance id [{characterId}].");
                yield break;
            }

            if (!_castAbilityDict.TryGetValue(abilityId, out var abilityAssetRef))
            {
                Debug.LogWarning($"Doesn't support ability [{abilityId}].");
                yield break;
            }

            var handle = abilityAssetRef.LoadAssetAsync();
            yield return handle;
            var ability = handle.Result;

            foreach (var skill in characterSkillHolder.Skills)
            {
                if (skill.Context.SkillInfo.Id != ability.Context.SkillInfo.Id) continue;
                Debug.Log($"Character already has ability [{abilityId}].");
                yield break;
            }

            List<CastSkillAbility> methodParams = new() { ability };
            MethodInfo dynMethod = characterSkillHolder.GetType().GetMethod("AddSkillsToHero",
                BindingFlags.NonPublic | BindingFlags.Instance);

            dynMethod.Invoke(characterSkillHolder, new object[] { methodParams });
            _castAbilitySpecDict.TryAdd(characterId, new Dictionary<int, GameplayAbilitySpec>());
            _castAbilitySpecDict[characterId].TryAdd(abilityId, character.AbilitySystem.GiveAbility(ability));
            Debug.Log($"Add ability [{abilityId}] to character [{character.DisplayName}] with id [{characterId}].");
        }

        private void RemoveCastAbilityFromCharacter(CommandArg[] args)
        {
            var abilityIds = ExtractAbilityIds(args);
            var characterId = args[1].Int;
            foreach (var abilityId in abilityIds) RemoveCastAbilityFromCharacter(characterId, abilityId);
        }

        private void RemoveCastAbilityFromCharacter(int characterId, int abilityId)
        {
            if (_castAbilitySpecDict.TryGetValue(characterId, out var characterAbilityDict) == false) return;
            if (characterAbilityDict.TryGetValue(abilityId, out var spec) == false) return;

            var character = CharacterCheats.Instance.GetCharacter(characterId);
            character.AbilitySystem.RemoveAbility(spec);

            Debug.Log(
                $"remove ability [{abilityId}] from character [{character.DisplayName}] with id [{characterId}].");
        }
    }
}