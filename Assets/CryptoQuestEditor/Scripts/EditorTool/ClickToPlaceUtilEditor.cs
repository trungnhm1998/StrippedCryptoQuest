using CryptoQuest.EditorTool;
using UnityEditor;
using UnityEngine;

namespace CryptoQuestEditor.EditorTool
{
    [CustomEditor(typeof(ClickToPlaceUtil))]
    public class ClickToPlaceUtilEditor : Editor
    {
        private ClickToPlaceUtil ClickToPlaceUtil => target as ClickToPlaceUtil;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Place at Mouse cursor") && !ClickToPlaceUtil.IsTargeting)
            {
                ClickToPlaceUtil.BeginTargeting();
                SceneView.duringSceneGui += DuringSceneGui;
            }
        }

        private void DuringSceneGui(SceneView sceneView)
        {
            var currentGUIEvent = Event.current;

            var mousePos = currentGUIEvent.mousePosition;
            var pixelsPerPoint = EditorGUIUtility.pixelsPerPoint;
            mousePos.y = sceneView.camera.pixelHeight - mousePos.y * pixelsPerPoint;
            mousePos.x *= pixelsPerPoint;

            var pos = sceneView.camera.ScreenToWorldPoint(mousePos);

            ClickToPlaceUtil.UpdateTargeting(pos);

            switch (currentGUIEvent.type)
            {
                case EventType.MouseMove:
                    HandleUtility.Repaint();
                    break;
                case EventType.MouseDown:
                    if (currentGUIEvent.button == 0)
                    {
                        ClickToPlaceUtil.EndTargeting();
                        SceneView.duringSceneGui -= DuringSceneGui;
                        currentGUIEvent.Use();
                    }

                    break;
            }
        }
    }
}