using CryptoQuest.Events;
using CryptoQuest.System.TransitionSystem;
using UnityEditor;
using UnityEngine;

namespace CryptoQuest
{
    [CustomEditor(typeof(TransitionEventChannelSO))]
    public class TransitionEventSOEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            GUI.enabled = Application.isPlaying;

            var eventSO = target as TransitionEventChannelSO;
            if (GUILayout.Button($"Raise {eventSO.name}"))
            {
                eventSO.RaiseEvent(eventSO.DebugValue);
            }
        }
    }
}