using CryptoQuest.UI.Menu.Panels.Option;
using UnityEditor;
using UnityEditor.UI;

namespace CryptoQuestEditor.UI
{
    [CustomEditor(typeof(UISlider))]
    public class UISliderEditor : SliderEditor
    {
        private SerializedProperty _steps;

        protected override void OnEnable()
        {
            base.OnEnable();
            _steps = serializedObject.FindProperty("_steps");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            EditorGUILayout.PropertyField(_steps);

            serializedObject.ApplyModifiedProperties();
        }
    }
}