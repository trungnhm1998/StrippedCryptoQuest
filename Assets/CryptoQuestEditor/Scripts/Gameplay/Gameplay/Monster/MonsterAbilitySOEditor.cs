using System;
using System.Collections.Generic;
using System.IO;
using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.Character.Enemy;
using IndiGames.Tools.ScriptableObjectBrowser;
using UnityEditor;
using UnityEngine;

namespace CryptoQuestEditor.Gameplay.Gameplay.Monster
{
    public class EnemySkillMapping : ScriptableObject { }

    public class MonsterAbilitySOEditor : ScriptableObjectBrowserEditor<EnemySkillMapping>
    {
        private const string DEFAULT_NAME = "Monster";
        private const int ROW_OFFSET = 2;
        private const int MONSTER_ID_COLUMN_INDEX = 0;
        private const int MONSTER_FIRST_ABILITY_COLUMN_INDEX = 28;
        private const int MONSTER_FIRST_PROBABILITY_COLUMN_INDEX = 29;
        private const string TARGET_TYPE_GROUP = "GroupOfEnemies";
        private const string TARGET_TYPE_ALL = "AllEnemies";
        private const string ENEMY_ABILITY_CLONE_PATH = "Assets/ScriptableObjects/Character/Skills/Enemy";

        private Dictionary<string, CastSkillAbility> _allAbilities;
        private Dictionary<string, EnemyDef> _allEnemies;
        private Dictionary<string, SkillTargetType> _allTargetType;

        public MonsterAbilitySOEditor()
        {
            CreateDataFolder = false;
            DefaultStoragePath = "Assets/ScriptableObjects/Character/Enemies";
        }


        public override void ImportBatchData(string directory, Action<ScriptableObject> callback)
        {
            string[] allLines = File.ReadAllLines(directory);
            _allAbilities = GetAllAbilities();
            _allEnemies = GetAllEnemies();
            _allTargetType = GetAllTargetType();

            for (int index = ROW_OFFSET; index < allLines.Length; index++)
            {
                // get data form tsv file
                string[] splittedData = allLines[index].Split('\t');

                string id = splittedData[MONSTER_ID_COLUMN_INDEX];
                if (string.IsNullOrEmpty(id) || id == "0")
                {
                    Debug.Log("Can't find monster id at row: " + index);
                    continue;
                }

                EnemyDef instance = GetEnemyDef(splittedData);
                if (instance == null)
                {
                    Debug.Log("Can't find enemy with id: " + splittedData[MONSTER_ID_COLUMN_INDEX]);
                    continue;
                }

                List<MonsterAbilitySpec> abilitySpecs = ConfigureMonsterAbilitySpec(splittedData);
                List<Skills> skillList = CreateSkillOnSpecs(abilitySpecs);
                SetEnemySkills(instance, skillList);
                EditorUtility.SetDirty(instance);
            }
        }


        private Dictionary<string, CastSkillAbility> GetAllAbilities()
        {
            Dictionary<string, CastSkillAbility> allAbilities = new();
            var guids = AssetDatabase.FindAssets("t:CastSkillAbility");
            foreach (var guid in guids)
            {
                var ability = AssetDatabase.LoadAssetAtPath<CastSkillAbility>(AssetDatabase.GUIDToAssetPath(guid));
                if (ability == null) continue;
                int id = ability.Context.SkillInfo.Id;
                if (ability.name != id.ToString()) continue;
                if (id != 0)
                    allAbilities.TryAdd(id.ToString(), ability);
            }

            return allAbilities;
        }

        private Dictionary<string, SkillTargetType> GetAllTargetType()
        {
            Dictionary<string, SkillTargetType> allTargetType = new();
            var guids = AssetDatabase.FindAssets("t:SkillTargetType");
            foreach (var guid in guids)
            {
                var targetType = AssetDatabase.LoadAssetAtPath<SkillTargetType>(AssetDatabase.GUIDToAssetPath(guid));
                if (targetType == null) continue;
                allTargetType.TryAdd(targetType.name, targetType);
            }

            return allTargetType;
        }

        private Dictionary<string, EnemyDef> GetAllEnemies()
        {
            Dictionary<string, EnemyDef> allEnemies = new();
            var guids = AssetDatabase.FindAssets("t:EnemyDef");
            foreach (var guid in guids)
            {
                var enemy = AssetDatabase.LoadAssetAtPath<EnemyDef>(AssetDatabase.GUIDToAssetPath(guid));
                if (enemy == null) continue;
                int id = enemy.Id;
                if (id != 0)
                    allEnemies.TryAdd(id.ToString(), enemy);
            }

            return allEnemies;
        }

        private List<MonsterAbilitySpec> ConfigureMonsterAbilitySpec(string[] splitedData)
        {
            List<MonsterAbilitySpec> monsterAbilitySpecs = new();
            for (int i = 0; i < 4; i++)
            {
                string abilityId = splitedData[MONSTER_FIRST_ABILITY_COLUMN_INDEX + i * 2];
                string abilityProbability = splitedData[MONSTER_FIRST_PROBABILITY_COLUMN_INDEX + i * 2];
                if (!string.IsNullOrEmpty(abilityId) && !string.IsNullOrEmpty(abilityProbability))
                {
                    monsterAbilitySpecs.Add(new MonsterAbilitySpec()
                    {
                        Id = abilityId,
                        Probability = float.Parse(abilityProbability) / 100f
                    });
                }
            }

            return monsterAbilitySpecs;
        }

        private EnemyDef GetEnemyDef(string[] datas)
        {
            string id = datas[MONSTER_ID_COLUMN_INDEX];
            return _allEnemies.TryGetValue(id, out var def) ? def : null;
        }

        private List<Skills> CreateSkillOnSpecs(List<MonsterAbilitySpec> specs)
        {
            List<Skills> skillList = new();
            foreach (var spec in specs)
            {
                if (_allAbilities.TryGetValue(spec.Id, out var ability))
                {
                    skillList.Add(new Skills()
                    {
                        Probability = spec.Probability,
                        SkillDef = ability
                    });
                }
            }

            return skillList;
        }

        private void SetEnemySkills(EnemyDef enemyDef, List<Skills> skills)
        {
            SerializedObject serializedObject = new SerializedObject(enemyDef);
            SerializedProperty skillsProperty = serializedObject.FindProperty("_skills");
            skillsProperty.ClearArray();
            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();

            for (var index = 0; index < skills.Count; index++)
            {
                var skill = skills[index];
                skillsProperty.InsertArrayElementAtIndex(index);
                var element = skillsProperty.GetArrayElementAtIndex(index);
                element.FindPropertyRelative("Probability").floatValue = skill.Probability;
                element.FindPropertyRelative("SkillDef").objectReferenceValue = skill.SkillDef;
            }

            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
        }

        private class MonsterAbilitySpec
        {
            public string Id;
            public float Probability;
        }
    }
}