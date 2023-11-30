using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace CryptoQuest.Quest.Components.PostCutsceneActionTrack
{
    [Serializable]
    public class PostCutsceneActionPlayableAsset : PlayableAsset, ITimelineClipAsset
    {
        [SerializeField] private PostCutsceneActionPlayableBehaviour _template;
        public ClipCaps clipCaps => ClipCaps.None;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<PostCutsceneActionPlayableBehaviour>.Create(graph, _template);

            return playable;
        }
    }
}