using UnityEditor;
using UnityEngine;

namespace CryptoQuestEditor.System.CutsceneSystem.CustomTimelineTracks.GameObjectPositionMarker
{
    [CustomEditor(
        typeof(CryptoQuest.System.CutsceneSystem.CustomTimelineTracks.GameObjectPosition.GameObjectPositionMarker))]
    public class GameObjectPositionEditor : Editor
    {
        private bool _authoring;

        private CryptoQuest.System.CutsceneSystem.CustomTimelineTracks.GameObjectPosition.GameObjectPositionMarker
            Marker => target as CryptoQuest.System.CutsceneSystem.CustomTimelineTracks.GameObjectPosition.
            GameObjectPositionMarker;

        private void OnEnable()
        {
            SceneView.duringSceneGui += DrawPositionHandler;
        }

        private void OnDisable()
        {
            SceneView.duringSceneGui -= DrawPositionHandler;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Center Gizmo"))
            {
                serializedObject.FindProperty("Position").vector3Value = SceneView.lastActiveSceneView.pivot;
                serializedObject.ApplyModifiedProperties();
            }
        }

        private void DrawPositionHandler(SceneView sceneView)
        {
            EditorGUI.BeginChangeCheck();
            var pos = Handles.PositionHandle(serializedObject.FindProperty("Position").vector3Value,
                Quaternion.identity);
            Handles.color = new Color(0, 255, 0, 0.05f);
            Handles.SphereHandleCap(0, pos, Quaternion.identity, .25f, EventType.Repaint);
            // off set label position y by it height
            pos.y -= .1f;
            Handles.Label(pos, serializedObject.FindProperty("Description").stringValue, new GUIStyle()
            {
                normal = new GUIStyleState()
                {
                    textColor = serializedObject.FindProperty("Color").colorValue,
                },
                alignment = TextAnchor.UpperCenter,
                fontSize = 20,
                fontStyle = FontStyle.Bold,
            });

            if (!EditorGUI.EndChangeCheck()) return;

            serializedObject.FindProperty("Position").vector3Value = pos;
            serializedObject.ApplyModifiedProperties();

            Repaint();
        }
    }
}