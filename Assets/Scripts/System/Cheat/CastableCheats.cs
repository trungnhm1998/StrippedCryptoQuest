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
        private readonly Dictionary<string, AssetReferenceT<CastSkillAbility>> _castAbilityDict = new();
        private readonly Dictionary<int, Dictionary<string, GameplayAbilitySpec>> _castAbilitySpecDict = new();

        [Serializable]
        private struct Castables
        {
            public string CastableName;
            public AssetReferenceT<CastSkillAbility> Ability;
        }

#if UNITY_EDITOR
        private const string CAST_SKILL_FOLDER = "Assets/ScriptableObjects/Character/Skills/Castables";
        private const string TEST_CAST_SKILL_FOLDER = "Assets/ScriptableObjects/Character/Skills/_WIP/TestSkills";

        private void OnValidate()
        {
            ValidateCastable();
            ValidateTestCastable();
        }

        private void ValidateTestCastable()
        {
            foreach (var (guid, cast) in LoadSkillAssetAtFolder(TEST_CAST_SKILL_FOLDER))
            {
                int id = cast.Context.SkillInfo.Id;
                Castables castables = new Castables()
                {
                    CastableName = cast.name,
                    Ability = new AssetReferenceT<CastSkillAbility>(guid)
                };
                _castables.Add(castables);
            }
        }


        private void ValidateCastable()
        {
            if (_castables.Count != 0) return;
            foreach (var (guid, cast) in LoadSkillAssetAtFolder(CAST_SKILL_FOLDER))
            {
                int id = cast.Context.SkillInfo.Id;
                Castables castables = new Castables()
                {
                    CastableName = cast.name,
                    Ability = new AssetReferenceT<CastSkillAbility>(guid)
                };
                _castables.Add(castables);
            }
        }

        private IEnumerable<(string, CastSkillAbility)> LoadSkillAssetAtFolder(string folderPath)
        {
            var abilityGuids = AssetDatabase.FindAssets("t:CastSkillAbility", new[] {folderPath});
            foreach (var guid in abilityGuids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var cast = AssetDatabase.LoadAssetAtPath<CastSkillAbility>(path);
                if (cast == null) continue;
                yield return (guid, cast);
            }
        }
#endif
        private void Awake()
        {
            foreach (var item in _castables)
            {
                _castAbilityDict.TryAdd(item.CastableName.ToLower(), item.Ability);
            }
        }

        public void InitCheats()
        {
            Terminal.Shell.AddCommand("add.cast", AddAbilityToCharacter, 2, 2,
                "add.cast <cast-able_id> <character_index>, add cast-able ability with id to character in party index");
            Terminal.Shell.AddCommand("remove.cast", RemoveCastAbilityFromCharacter, 2, 2,
                "remove.cast <cast-able_id> <character_index>, remove cast-able with id from character in party index");

            foreach (var ability in _castAbilityDict)
            {
                Terminal.Autocomplete.Register(ability.Key);
            }
        }

        private void AddAbilityToCharacter(CommandArg[] args)
        {
            var abilityIds = ExtractAbilityIds(args);
            var characterId = args[1].Int;
            foreach (var abilityId in abilityIds)
                StartCoroutine(LoadThenAddAbilityToCharacterCo(characterId, abilityId));
        }

        private static string[] ExtractAbilityIds(CommandArg[] args)
        {
            return args[0].String.Split(',');
        }

        private IEnumerator LoadThenAddAbilityToCharacterCo(int characterId, string abilityId)
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
            _castAbilitySpecDict.TryAdd(characterId, new());
            _castAbilitySpecDict[characterId].TryAdd(abilityId, character.AbilitySystem.GiveAbility(ability));
            Debug.Log($"Add ability [{abilityId}] to character [{character.DisplayName}] with id [{characterId}].");
        }

        private void RemoveCastAbilityFromCharacter(CommandArg[] args)
        {
            var abilityIds = ExtractAbilityIds(args);
            var characterId = args[1].Int;
            foreach (var abilityId in abilityIds) RemoveCastAbilityFromCharacter(characterId, abilityId);
        }

        private void RemoveCastAbilityFromCharacter(int characterId, string abilityId)
        {
            if (_castAbilitySpecDict.TryGetValue(characterId, out var characterAbilityDict) == false) return;
            if (characterAbilityDict.TryGetValue(abilityId, out var spec) == false) return;

            var character = CharacterCheats.Instance.GetCharacter(characterId);
            var characterSkillHolder = character.GetComponent<HeroSkills>();

            List<CastSkillAbility> currentSkills = new(characterSkillHolder.Skills);
            foreach (var currentSkill in currentSkills)
            {
                if (currentSkill.Context.SkillInfo.Id.ToString() != abilityId) continue;
                currentSkills.Remove(currentSkill);
                break;
            }

            var property = typeof(HeroSkills).GetProperty("Skills", BindingFlags.NonPublic | BindingFlags.Instance);
            if (property == null) return;
            property.SetValue(characterSkillHolder, currentSkills);

            character.AbilitySystem.RemoveAbility(spec);

            Debug.Log(
                $"remove ability [{abilityId}] from character [{character.DisplayName}] with id [{characterId}].");
        }
    }
}