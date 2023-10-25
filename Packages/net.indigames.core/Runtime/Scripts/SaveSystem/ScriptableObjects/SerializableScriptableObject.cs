using IndiGames.Core.EditorTools.Attributes.ReadOnlyAttribute;
using UnityEditor;
using UnityEngine;

namespace IndiGames.Core.SaveSystem.ScriptableObjects
{
    public class SerializableScriptableObject : ScriptableObject
    {
        [SerializeField, ReadOnly] private string _guid;
        public string Guid => _guid;

        private void OnEnable()
        {
            ScriptableObjectRegistry.AddScriptableObject(this);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            var assetPath = AssetDatabase.GetAssetPath(this);
            _guid = AssetDatabase.AssetPathToGUID(assetPath);
        }
#endif
    }
}