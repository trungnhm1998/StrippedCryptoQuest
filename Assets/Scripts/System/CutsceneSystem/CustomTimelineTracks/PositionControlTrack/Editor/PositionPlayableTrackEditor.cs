using CryptoQuest.Timeline.Position;
using UnityEditor;
using UnityEngine;

namespace CryptoQuestEditor.Timeline.Position
{
    [CustomEditor(typeof(PositionPlayableTrack))]
    public class PositionPlayableTrackEditor : Editor
    {
        private const float HEADER_HEIGHT = 21f;
        private SerializedProperty m_Name; // unity internal name property

        protected override void OnHeaderGUI()
        {
            base.OnHeaderGUI();

            // draw a name edit field to edit and save the track name
            using (new GUILayout.HorizontalScope())
            {
                EditorGUI.BeginChangeCheck();
                var newName = EditorGUILayout.DelayedTextField(m_Name.stringValue);
                if (EditorGUI.EndChangeCheck())
                {
                    m_Name.stringValue = newName;
                    serializedObject.ApplyModifiedProperties();
                }
            }

            GUILayout.Space(HEADER_HEIGHT);
        }

        private void OnEnable()
        {
            m_Name = serializedObject.FindProperty("m_Name");
        }
    }
}