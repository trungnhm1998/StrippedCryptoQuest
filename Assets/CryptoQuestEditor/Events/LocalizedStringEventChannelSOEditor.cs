using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Events;
using UnityEditor;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest
{
    [CustomEditor(typeof(LocalizedStringEventChannelSO))]
    public class LocalizedStringEventChannelSOEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            GUI.enabled = Application.isPlaying;

            var eventSO = target as LocalizedStringEventChannelSO;
            if (GUILayout.Button($"Raise {eventSO.name}"))
            {
                eventSO.RaiseEvent(eventSO.DebugValue);
            }
        }
    }
}