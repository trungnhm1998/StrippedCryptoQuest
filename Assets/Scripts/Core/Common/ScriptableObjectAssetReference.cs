using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

namespace Core.Common
{
    [Serializable]
    public class ScriptableObjectAssetReference<TScriptableObject> : AssetReferenceT<TScriptableObject>
        where TScriptableObject : ScriptableObject
    {
        public ScriptableObjectAssetReference(string guid) : base(guid) { }

        public override bool ValidateAsset(Object obj)
        {
            return obj as TScriptableObject != null;
        }

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