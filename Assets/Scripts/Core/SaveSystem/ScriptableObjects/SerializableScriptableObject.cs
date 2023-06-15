using EditorTools.Attributes.ReadOnlyAttribute;
using UnityEditor;
using UnityEngine;

namespace Core.SaveSystem.ScriptableObjects
{
    public class SerializableScriptableObject : ScriptableObject
    {
        [SerializeField, ReadOnly] private string _guid;
        public string Guid => _guid;

#if UNITY_EDITOR
        private void OnValidate()
        {
            var assetPath = AssetDatabase.GetAssetPath(this);
            _guid = AssetDatabase.AssetPathToGUID(assetPath);
        }
#endif
    }
}