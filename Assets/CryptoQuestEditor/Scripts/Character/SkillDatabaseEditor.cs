using System.Collections.Generic;
using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.Character;
using CryptoQuest.Character.Skill;
using CsvHelper;
using CsvHelper.Configuration.Attributes;
using IndiGamesEditor.Core.Database;
using UnityEditor;

namespace CryptoQuestEditor.Character
{
    public class CsvSkill
    {
        [Name("physical_skill_id")] public int Id { get; set; }

        /*
          "1: Myself
           2: One ally
           3: All allies
           4: One enemy
           5: A group of enemies
           6: All enemies"
         */
        [Name("target_1")] public int TargetId { get; set; }
    }

    [CustomEditor(typeof(SkillDatabase))]
    public class SkillDatabaseEditor : DatabaseImporterEditor<SkillDatabase, int, CastSkillAbility, CsvSkill>
    {
        // Assets/ScriptableObjects/Character/Skills/TargetTypes/AllAllies.asset
        private const string TargetTypePath = "Assets/ScriptableObjects/Character/Skills/TargetTypes/";
        private static Dictionary<int, string> TargetType = new()
        {
            { 1, $"{TargetTypePath}Myself.asset" },
            { 2, $"{TargetTypePath}OneAlly.asset" },
            { 3, $"{TargetTypePath}AllAllies.asset" },
            { 4, $"{TargetTypePath}OneEnemy.asset" },
            { 5, $"{TargetTypePath}GroupOfEnemies.asset" },
            { 6, $"{TargetTypePath}AllEnemies.asset" },
        };
        protected override string ExportPath => "Assets/ScriptableObjects/Character/Skills/WIP";

        public SkillDatabase Target => target as SkillDatabase;

        protected override int GetId(CsvSkill data) => data.Id;

        protected override void OnAssetModified(ref SerializedObject serializeObjectInstance, CastSkillAbility asset,
            CsvSkill data)
        {
            serializeObjectInstance.FindProperty("<Parameters>k__BackingField.Id").intValue = data.Id;
            serializeObjectInstance.FindProperty("<TargetType>k__BackingField").objectReferenceValue =
                AssetDatabase.LoadAssetAtPath(TargetType[data.TargetId], typeof(SkillTargetType)) as SkillTargetType;
        }

        protected override string GetAssetName(CsvSkill data) => "Test_" + data.Id;

        protected override bool IgnoreRow(ReadingContext contextRawRecord)
        {
            return contextRawRecord.Row == 2;
        }
    }
}