using CryptoQuest.DialogueSystem;
using CryptoQuest.Events;
using CryptoQuest.Events.Gameplay;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using IndiGames.GameplayAbilitySystem.AbilitySystem.ScriptableObjects;


namespace CryptoQuest
{
#if UNITY_EDITOR
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

#endif
}