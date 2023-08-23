using CryptoQuest.UI.Menu.Panels.Status;
using CryptoQuest.UI.Menu.Panels.Status.Equipment;
using UnityEditor;
using UnityEditor.UI;

namespace CryptoQuestEditor.UI.Menu
{
    [CustomEditor(typeof(UIEquipmentSlotButton))]
    public class UIEquipmentSlotButtonEditor : ButtonEditor
    {
        private SerializedProperty _selectEffect;

        protected override void OnEnable()
        {
            base.OnEnable();
            _selectEffect = serializedObject.FindProperty("_selectEffect");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            EditorGUILayout.PropertyField(_selectEffect);

            serializedObject.ApplyModifiedProperties();
        }
    }
}