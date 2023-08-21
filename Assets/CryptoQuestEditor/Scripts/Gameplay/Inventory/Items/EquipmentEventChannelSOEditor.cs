using CryptoQuest.Events.Gameplay;
using UnityEditor;
using UnityEngine;

namespace CryptoQuestEditor.Gameplay.Inventory
{
    [CustomEditor(typeof(EquipmentEventChannelSO))]
    public class EquipmentEventChannelSOEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            GUI.enabled = Application.isPlaying;
            
            var eventSO = target as EquipmentEventChannelSO;
            if (GUILayout.Button($"Raise {eventSO.name}"))
            {
                eventSO.RaiseEvent(eventSO.SlotType, eventSO.Equipment);
            }
        }
    }
}