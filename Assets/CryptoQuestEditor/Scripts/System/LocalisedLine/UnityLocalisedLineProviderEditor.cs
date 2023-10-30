#if UNITY_EDITOR
using UnityEditor;

namespace CryptoQuest.System.Dialogue.YarnManager
{
    [CustomEditor(typeof(UnityLocalisedLineProvider))]
    public class UnityLocalisedLineProviderEditor : Editor
    {
        private SerializedProperty stringsTableProperty;
        private SerializedProperty assetTableProperty;

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(stringsTableProperty);

            var stringTableName = stringsTableProperty.FindPropertyRelative("m_TableReference")
                .FindPropertyRelative("m_TableCollectionName").stringValue;

            if (string.IsNullOrEmpty(stringTableName))
            {
                EditorGUI.indentLevel += 1;
                EditorGUILayout.HelpBox("Choose a strings table to make this line provider able to deliver line text.",
                    MessageType.Warning);
                EditorGUI.indentLevel -= 1;
            }

            EditorGUILayout.PropertyField(assetTableProperty);

            serializedObject.ApplyModifiedProperties();
        }

        public void OnEnable()
        {
#if USE_UNITY_LOCALIZATION
            this.stringsTableProperty = serializedObject.FindProperty(nameof(UnityLocalisedLineProvider.StringsTable));
            this.assetTableProperty = serializedObject.FindProperty(nameof(UnityLocalisedLineProvider.AssetTable));
#endif
        }
    }
}
#endif