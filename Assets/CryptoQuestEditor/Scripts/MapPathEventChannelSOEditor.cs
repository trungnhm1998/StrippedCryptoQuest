using CryptoQuest.Events;
using CryptoQuest.Map;
using UnityEditor;
using UnityEngine;

namespace CryptoQuestEditor
{
    [CustomEditor(typeof(MapPathEventChannelSO))]
    public class MapPathEventChannelSOEditor : Editor
    {
        private MapPathSO _object;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            GUI.enabled = Application.isPlaying;
            GUILayout.Label("Editor");
            GUILayout.BeginVertical();
            {
                GUILayout.BeginHorizontal();

                GUILayout.Label("Map path object");
                _object = EditorGUILayout.ObjectField(_object, typeof(MapPathSO)) as MapPathSO;
                GUILayout.EndHorizontal();
            }
            MapPathEventChannelSO eventSO = target as MapPathEventChannelSO;
            if (GUILayout.Button($"Raise {eventSO.name}"))
            {
                eventSO.RaiseEvent(_object);
            }

            GUILayout.EndVertical();
        }
    }
}