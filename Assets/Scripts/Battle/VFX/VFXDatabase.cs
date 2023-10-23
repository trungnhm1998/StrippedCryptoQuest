using System;
using IndiGames.Core.Database;
using UnityEngine;
using UnityEngine.AddressableAssets;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CryptoQuest.Battle.VFX
{
    public class VFXDatabase : AssetReferenceDatabaseT<int, GameObject>
    {
#if UNITY_EDITOR
        private const string VFX_PATH = "Assets/Plugins/MegaPack VFX/Prefabs";

        public override void Editor_FetchDataInProject()
        {
            _maps = Array.Empty<Map>();

            var guids = AssetDatabase.FindAssets("t:prefab", new[] { VFX_PATH });

            foreach (var guid in guids)
            {
                var instance = new Map();
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var asset = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                if (Editor_Validate((asset, path)) == false) continue;
                var assetRef = new AssetReferenceT<GameObject>(guid);
                assetRef.SetEditorAsset(asset);
                instance.Id = Editor_GetInstanceId(asset);
                instance.Data = assetRef;
                ArrayUtility.Add(ref _maps, instance);
            }
        }

        protected override int Editor_GetInstanceId(GameObject vfx)
        {
            return int.Parse(vfx.name.Split("_")[0]);
        }
#endif
    }
}