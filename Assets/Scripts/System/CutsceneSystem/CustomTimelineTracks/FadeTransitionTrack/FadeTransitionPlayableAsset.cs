using System;
using CryptoQuest.System.CutsceneSystem.CustomTimelineTracks.YarnSpinnerNodeControlTrack;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace CryptoQuest.System.CutsceneSystem.CustomTimelineTracks.FadeTransitionTrack
{
    [Serializable]
    public class FadeTransitionPlayableAsset : PlayableAsset, ITimelineClipAsset
    {
        public float BlackScreenDuration = 1f;
        [SerializeField] private YarnSpinnerNodePlayableBehaviour _template;
        public ClipCaps clipCaps => ClipCaps.None;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<YarnSpinnerNodePlayableBehaviour>.Create(graph, _template);
            // _template.BlackScreenDuration = BlackScreenDuration;

            return playable;
        }
    }
}