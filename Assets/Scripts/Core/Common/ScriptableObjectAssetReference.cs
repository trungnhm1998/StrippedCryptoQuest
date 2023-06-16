using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Core.Common
{
    [Serializable]
    public class ScriptableObjectAssetReference<TScriptableObject> : AssetReference
        where TScriptableObject : ScriptableObject
    {
        public ScriptableObjectAssetReference(string guid) : base(guid) { }

        public override bool ValidateAsset(string path)
        {
#if UNITY_EDITOR
            var so = AssetDatabase.LoadAssetAtPath<TScriptableObject>(path);
            return so != null;
#else
            return false;
#endif
        }
    }
}