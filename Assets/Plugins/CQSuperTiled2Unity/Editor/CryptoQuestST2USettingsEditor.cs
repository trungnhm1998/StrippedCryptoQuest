using SuperTiled2Unity.Editor;
using UnityEditor;

namespace CryptoQuestEditor.SuperTiled2Unity
{
    [CustomEditor(typeof(ST2USettings))]
    public class CryptoQuestST2USettingsEditor : ST2USettingsEditor
    {
        private SerializedProperty _typePrefabReplacement;

        private void OnEnable()
        {
            _typePrefabReplacement = serializedObject.FindProperty("_customPrefabReplacements");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();
            EditorGUILayout.PropertyField(_typePrefabReplacement, true);
            serializedObject.ApplyModifiedProperties();
        }
    }
}