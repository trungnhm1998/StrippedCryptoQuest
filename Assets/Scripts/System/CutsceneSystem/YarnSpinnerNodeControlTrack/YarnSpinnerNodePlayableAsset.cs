using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace CryptoQuest.System.CutsceneSystem.YarnSpinnerNodeControlTrack
{
    [Serializable]
    public class YarnSpinnerNodePlayableAsset : PlayableAsset, ITimelineClipAsset
    {
        [SerializeField] private string _yarnNodeName;
        [SerializeField] private YarnSpinnerNodePlayableBehaviour _template;

        public ClipCaps clipCaps => ClipCaps.None;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<YarnSpinnerNodePlayableBehaviour>.Create(graph, _template);
            _template.YarnNodeName = _yarnNodeName;
            return playable;
        }
    }
}