using System;
using CryptoQuest.AbilitySystem.Abilities;
using IndiGames.Core.Database;
#if UNITY_EDITOR
    using UnityEditor;
using UnityEngine.AddressableAssets;
#endif

namespace CryptoQuest.Character
{
    public class SkillDatabase : AssetReferenceDatabaseT<int, CastSkillAbility>
    {
#if UNITY_EDITOR
        protected override int Editor_GetInstanceId(CastSkillAbility asset) => asset.SkillInfo.Id;

        protected override bool Editor_Validate((CastSkillAbility asset, string path) data)
        {
            return data.asset.SkillInfo.Id != 0;
        }

        private const string CAST_SKILL_FOLDER = "Assets/ScriptableObjects/Character/Skills/Castables";
        public override void Editor_FetchDataInProject()
        {
            _maps = Array.Empty<Map>();

            var assetUids = AssetDatabase.FindAssets("t:CastSkillAbility", new[] {CAST_SKILL_FOLDER});

            foreach (var uid in assetUids)
            {
                var instance = new Map();
                var path = AssetDatabase.GUIDToAssetPath(uid);
                var asset = AssetDatabase.LoadAssetAtPath<CastSkillAbility>(path);
                if (Editor_Validate((asset, path)) == false) continue;

                var assetRef = new AssetReferenceT<CastSkillAbility>(uid);

                assetRef.SetEditorAsset(asset);
                instance.Id = Editor_GetInstanceId(asset);
                instance.Data = assetRef;
                ArrayUtility.Add(ref _maps, instance);
            }
        }
#endif
    }
}