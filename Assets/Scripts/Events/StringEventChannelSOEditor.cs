using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CryptoQuest
{
    [CustomEditor(typeof(StringEventChannelSO))]
    public class StringEventChannelSOEditor : UnityEditor.Editor
    {
        public string value = "test";

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUI.enabled = Application.isPlaying;

            var eventSO = target as StringEventChannelSO;
            if (GUILayout.Button($"Raise {eventSO.name}"))
                eventSO.RaiseEvent(value);
        }
    }
}