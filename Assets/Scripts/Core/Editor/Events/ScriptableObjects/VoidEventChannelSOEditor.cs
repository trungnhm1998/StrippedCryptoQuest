namespace Core.Editor.Events.ScriptableObjects
{
    using Core.Runtime.Events.ScriptableObjects;
    using UnityEngine;
    using UnityEditor;

    [CustomEditor(typeof(VoidEventChannelSO))]
    public class VoidEventChannelSOEditor : Editor
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