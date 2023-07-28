using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Yarn.Unity;

namespace CryptoQuest.System.CutsceneSystem.CustomTimelineTracks.YarnSpinnerNodeControlTrack
{
    [Serializable]
    public class YarnSpinnerNodePlayableAsset : PlayableAsset, ITimelineClipAsset
    {
        [HideInInspector] public YarnProject YarnProj;
        [HideInInspector] public string YarnNodeName = "Start";
        [SerializeField] private YarnSpinnerNodePlayableBehaviour _template;

        public ClipCaps clipCaps => ClipCaps.None;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<YarnSpinnerNodePlayableBehaviour>.Create(graph, _template);
            _template.YarnNodeName = YarnNodeName;
            return playable;
        }
    }
}