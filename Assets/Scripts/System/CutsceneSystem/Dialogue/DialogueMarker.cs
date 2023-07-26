using CryptoQuest.Gameplay.Quest.Dialogue.ScriptableObject;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace CryptoQuest.System.CutScene.Dialogue
{
    [CustomStyle("AnnotationMarker")]
    public class DialogueMarker : Marker, INotification
    {
        public PropertyName id { get; }

        [Header("Editor")]
        public Color color = new Color(1.0f, 1.0f, 1.0f, 0.5f);

        public bool showLineOverlay = true;

        [Header("Dialogue Option")]
        public DialogueScriptableObject DialogueLineEvent;

        public VoidEventChannelSO EmotionEvent;
    }
}