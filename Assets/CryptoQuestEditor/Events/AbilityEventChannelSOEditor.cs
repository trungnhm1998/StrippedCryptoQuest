using CryptoQuest.Events.Gameplay;
using UnityEngine;
using UnityEditor;

namespace CryptoQuest
{
    [CustomEditor(typeof(AbilityEventChannelSO))]
    public class AbilityEventChannelSOEditor : Editor
    {
        private SerializedProperty _abilitySOProperty;

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(_abilitySOProperty);
            serializedObject.ApplyModifiedProperties();

            var eventSO = target as AbilityEventChannelSO;
            if (GUILayout.Button($"Raise {eventSO.name}"))
            {
                eventSO.RaiseEvent(eventSO.AbilitySO);
            }
        }

        public void OnEnable()
        {
            _abilitySOProperty = serializedObject.FindProperty(nameof(AbilityEventChannelSO.AbilitySO));
        }
    }
}