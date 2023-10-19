using System;
using CryptoQuest.Quest.Actions;
using CryptoQuest.System.CutsceneSystem.CustomTimelineTracks.PostCutsceneActionTrack;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Serialization;
using UnityEngine.Timeline;

namespace CryptoQuest.System.CutsceneSystem.CustomTimelineTracks
{
    [Serializable]
    public class PostCutsceneActionPlayableAsset : PlayableAsset, ITimelineClipAsset
    {
        [SerializeField] private PlayableAsset _playableAsset;
        [SerializeField] private PostCutsceneActionPlayableBehaviour _template;
        public ClipCaps clipCaps => ClipCaps.None;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<PostCutsceneActionPlayableBehaviour>.Create(graph, _template);
            var director = owner.GetComponent<PlayableDirector>();

            return playable;
        }
    }
}