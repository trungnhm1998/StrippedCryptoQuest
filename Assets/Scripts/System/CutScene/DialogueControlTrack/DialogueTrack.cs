using System;
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
        [SerializeField] private DialogueEventChannelSO _playDialogueEvent;

        [SerializeField] private VoidEventChannelSO _pauseTimelineEvent;

        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            foreach (TimelineClip clip in GetClips())
            {
                DialogueClip clipControl = clip.asset as DialogueClip;

                if (clipControl == null)
                {
                    Debug.LogWarning("This clip contains no DialogueLine");
                    continue;
                }

                clipControl.pauseTimelineEvent = _pauseTimelineEvent;
                clipControl.playDialogueEvent = _playDialogueEvent;
            }

            return base.CreateTrackMixer(graph, go, inputCount);
        }
    }
}