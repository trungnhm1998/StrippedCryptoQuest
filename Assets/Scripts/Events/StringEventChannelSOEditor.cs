using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CryptoQuest
{
    [CustomEditor(typeof(StringEventChannelSO))]
    public class StringEventChannelSOEditor : UnityEditor.Editor
    {
        private string stringValue = "";

        private void OnEnable() { }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            GUI.enabled = Application.isPlaying;
            GUILayout.Label("Editor");
            GUILayout.BeginVertical();
            {
                GUILayout.BeginHorizontal();

                GUILayout.Label("String Value");
                stringValue = EditorGUILayout.TextField(stringValue);
                GUILayout.EndHorizontal();
            }
            var eventSO = target as StringEventChannelSO;
            if (GUILayout.Button($"Raise {eventSO.name}"))
            {
                eventSO.RaiseEvent(stringValue);
            }
            GUILayout.EndVertical();
        }
    }
}