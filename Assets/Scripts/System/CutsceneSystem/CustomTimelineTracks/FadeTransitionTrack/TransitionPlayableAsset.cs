using System;
using CryptoQuest.System.CutsceneSystem.CustomTimelineTracks.YarnSpinnerNodeControlTrack;
using CryptoQuest.System.TransitionSystem;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace CryptoQuest.System.CutsceneSystem.CustomTimelineTracks.FadeTransitionTrack
{
    [Serializable]
    public class TransitionPlayableAsset : PlayableAsset, ITimelineClipAsset
    {
        // public float BlackScreenDuration = 1f;
        [SerializeField] private TransitionEventChannelSO _transitionEventChannelSO;
        [field: SerializeReference] public AbstractTransition Transition { get; set; }
        [SerializeField] private TransitionPlayableBehavior _template;
        public ClipCaps clipCaps => ClipCaps.None;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<TransitionPlayableBehavior>.Create(graph, _template);
            _template.Transition = Transition;
            _template.TransitionEvent = _transitionEventChannelSO;

            return playable;
        }
    }
}