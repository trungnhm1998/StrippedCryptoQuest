using CryptoQuest.Timeline.Position;
using UnityEditor;
using UnityEngine;

namespace CryptoQuestEditor.Timeline.Position
{
    [CustomEditor(typeof(PositionClip))]
    public class PositionClipEditor : Editor
    {
        private void OnEnable()
        {
            SceneView.duringSceneGui += DuringSceneGUI;
        }

        private void OnDisable()
        {
            SceneView.duringSceneGui -= DuringSceneGUI;
        }

        private void DuringSceneGUI(SceneView sceneView)
        {
            EditorGUI.BeginChangeCheck();

            var gizmoPosition = Handles.PositionHandle(((PositionClip)target).Position, Quaternion.identity);

            if (!EditorGUI.EndChangeCheck()) return;

            Undo.RecordObject(target, "Changed Position");
            serializedObject.FindProperty("Position").vector3Value = gizmoPosition;
            serializedObject.ApplyModifiedProperties();
            Repaint();
        }
    }
}