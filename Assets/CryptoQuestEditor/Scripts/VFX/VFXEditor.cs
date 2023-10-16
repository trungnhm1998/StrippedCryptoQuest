using CryptoQuest.Battle.VFX;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;

namespace CryptoQuestEditor.VFX
{
    public class VFXEditor : Editor
    {
        private const string DATA_PATH = "Assets/ScriptableObjects/Battle/VFX/Vfxs/";
        private const string VFX_PATH = "Assets/Plugins/MegaPack VFX/Prefabs";
        [MenuItem("Crypto Quest/Generate VFX")]
        public static void GenerateVFX()
        {
            string[] files = Directory.GetFiles(VFX_PATH, "*.prefab", SearchOption.TopDirectoryOnly);

            Debug.Log("Start Generate VFX " + files.Length);

            foreach (var file in files)
            {
                var prefab = AssetDatabase.LoadAssetAtPath(file, typeof(GameObject));
                if(prefab != null)
                {
                    Debug.Log("Generate " + prefab.name);

                    string id = prefab.name.Substring(0, prefab.name.IndexOf('_'));

                    VFXDataSO instance;

                    if (id == "65") // Ref to origin project that vfx 65 is move vfx
                    {
                        instance = ScriptableObject.CreateInstance<VFXMoveDataSO>();
                    }
                    else
                    {
                        instance = ScriptableObject.CreateInstance<VFXDataSO>();
                    }    

                    instance.name = prefab.name;
                    instance.Id = id;
                    instance.VfxPrefab = (GameObject)prefab;
                    AssetDatabase.CreateAsset(instance, DATA_PATH + instance.name + ".asset");

                    UnityEditor.EditorUtility.SetDirty(instance);
                }    
            }
        }
    }
}
