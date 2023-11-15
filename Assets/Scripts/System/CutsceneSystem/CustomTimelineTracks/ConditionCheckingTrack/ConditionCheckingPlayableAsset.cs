using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace CryptoQuest.System.CutsceneSystem.CustomTimelineTracks.ConditionCheckingTrack
{
    [Serializable]
    public class ConditionCheckingPlayableAsset : PlayableAsset, ITimelineClipAsset
    {
        [SerializeField]
        private ConditionCheckingTrackPlayableBehavior _template = new();

        public ClipCaps clipCaps => ClipCaps.None;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            ScriptPlayable<ConditionCheckingTrackPlayableBehavior> playable =
                ScriptPlayable<ConditionCheckingTrackPlayableBehavior>.Create(graph, _template);
            PlayableDirector director = graph.GetResolver() as PlayableDirector;
            _template.SetDirector(director);
            return playable;
        }
    }
}