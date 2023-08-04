using CryptoQuest.UI.Menu;
using UnityEditor;
using UnityEditor.UI;

namespace CryptoQuestEditor.UI.Menu
{
    [CustomEditor(typeof(UIHeaderButton))]
    public class UIHeaderButtonEditor : ButtonEditor
    {
        private SerializedProperty _typeSO;
        private SerializedProperty _pointer;
        private SerializedProperty _header;
        private SerializedProperty _normalColor;
        private SerializedProperty _disabledColor;

        protected override void OnEnable()
        {
            base.OnEnable();
            _typeSO = serializedObject.FindProperty("_typeSO");
            _pointer = serializedObject.FindProperty("_pointer");
            _header = serializedObject.FindProperty("_header");
            _disabledColor = serializedObject.FindProperty("_disabled");
            _normalColor = serializedObject.FindProperty("_normal");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            EditorGUILayout.PropertyField(_typeSO);
            EditorGUILayout.PropertyField(_pointer);
            EditorGUILayout.PropertyField(_header);
            EditorGUILayout.PropertyField(_normalColor);
            EditorGUILayout.PropertyField(_disabledColor);

            serializedObject.ApplyModifiedProperties();
        }
    }
}