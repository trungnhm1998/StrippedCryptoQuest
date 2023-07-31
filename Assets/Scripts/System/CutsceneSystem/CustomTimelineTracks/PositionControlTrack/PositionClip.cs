using System;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace CryptoQuest.Timeline.Position
{
    [Serializable]
#if UNITY_EDITOR
    [DisplayName("Position Clip")]
#endif
    public class PositionClip : PlayableAsset, ITimelineClipAsset
    {
        public Vector3 Position;

        [NotKeyable]
        public PositionPlayableBehaviour Template = new PositionPlayableBehaviour();

        public ClipCaps clipCaps => ClipCaps.None;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<PositionPlayableBehaviour>.Create(graph, Template);
            Template.Position = Position;
            return playable;
        }
    }
}