using CryptoQuest.System.CutScene.Events;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace CryptoQuestEditor.System.Cutscene
{
    [CustomEditor(typeof(CutsceneEventChannelSO))]
    public class CutsceneEventChannelSOEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUI.enabled = Application.isPlaying;

            var eventSO = target as CutsceneEventChannelSO;
            if (GUILayout.Button($"Raise {eventSO.name}"))
                eventSO.RaiseEvent();
        }
    }
}