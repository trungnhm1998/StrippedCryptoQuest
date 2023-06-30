using IndiGames.Core.Events.ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace IndiGames.Core.Editor
{
    [CustomEditor(typeof(VoidEventChannelSO))]
    public class VoidEventChannelSOEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUI.enabled = Application.isPlaying;

            var eventSO = target as VoidEventChannelSO;
            if (GUILayout.Button($"Raise {eventSO.name}"))
                eventSO.RaiseEvent();
        }
    }
}