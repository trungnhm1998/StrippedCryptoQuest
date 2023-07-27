using UnityEditor;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace CryptoQuestEditor.CryptoQuestEditor.System.CutsceneSystem.CustomTimelineTracks.GameObjectPositionMarker
{
    [CustomEditor(
        typeof(CryptoQuest.System.CutsceneSystem.CustomTimelineTracks.GameObjectPosition.GameObjectPositionMarker))]
    public class GameObjectPositionGizmoEditor : Editor
    {
        private void OnEnable()
        {
            SceneView.duringSceneGui += DrawPositionHandler;
        }

        private void OnDisable()
        {
            SceneView.duringSceneGui -= DrawPositionHandler;
        }

        private void DrawPositionHandler(SceneView sceneView)
        {
            EditorGUI.BeginChangeCheck();
            Vector3 gizmoPos = Handles.PositionHandle(serializedObject.FindProperty("Position").vector3Value,
                Quaternion.identity);
            // draw a rectangle at the position
            Handles.DrawWireCube(gizmoPos, Vector3.one * 0.5f);

            if (!EditorGUI.EndChangeCheck()) return;

            serializedObject.FindProperty("Position").vector3Value = gizmoPos;
            serializedObject.ApplyModifiedProperties();

            Repaint();
        }
    }
}