using CryptoQuest.Events.UI;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace CryptoQuest.System.CutScene.DialogueControlTrack
{
    [TrackClipType(typeof(DialogueClip))]
    public class DialogueTrack : PlayableTrack
    {
        [Header("Raise event")]
        public DialogEventChannelSO PlayDialogueEvent;
        public VoidEventChannelSO PauseTimelineEvent;

        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            foreach (TimelineClip clip in GetClips())
            {
                DialogueClip clipControl = clip.asset as DialogueClip;

                clipControl.PauseTimelineEvent = PauseTimelineEvent;
                clipControl.PlayDialogueEvent = PlayDialogueEvent;
            }

            return base.CreateTrackMixer(graph, go, inputCount);
        }
    }
}