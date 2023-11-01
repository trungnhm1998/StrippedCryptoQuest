using System;
using CryptoQuest.System.CutsceneSystem.CustomTimelineTracks.FadeTransitionTrack;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace CryptoQuest.System.CutsceneSystem.CustomTimelineTracks.ConditionCheckingTrack
{
    [Serializable]
    public class ConditionCheckingPlayableAsset : PlayableAsset, ITimelineClipAsset
    {
        [SerializeField] private ConditionCheckingTrackPlayableBehavior _template;
        public ClipCaps clipCaps => ClipCaps.None;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<ConditionCheckingTrackPlayableBehavior>.Create(graph, _template);

            return playable;
        }
    }
}