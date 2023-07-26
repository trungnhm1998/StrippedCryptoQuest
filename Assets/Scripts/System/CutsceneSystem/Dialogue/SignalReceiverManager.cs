using CryptoQuest.Events.UI;
using CryptoQuest.Gameplay.Quest.Dialogue.ScriptableObject;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;
using UnityEngine.Playables;

namespace CryptoQuest.System.CutsceneSystem.Dialogue
{
    public class SignalReceiverManager : MonoBehaviour, INotificationReceiver
    {
        [Header("Raise event")]
        [SerializeField] private DialogueEventChannelSO _playDialogueEvent;

        [SerializeField] private VoidEventChannelSO _pauseTimelineEvent;


        public void OnNotify(Playable origin, INotification notification, object context)
        {
            if (!(notification is DialogueMarker marker)) return;

            if (!IsPlaying(origin)) return;

            PauseTimeline();

            ShowDialogue(marker.DialogueLineEvent);
        }

        private void ShowDialogue(DialogueScriptableObject dialogueLine)
        {
            if (_playDialogueEvent != null) _playDialogueEvent.Show(dialogueLine);
        }

        private void PauseTimeline() => _pauseTimelineEvent.RaiseEvent();

        private bool IsPlaying(Playable playable) => Application.isPlaying && playable.GetGraph().IsPlaying();
    }
}