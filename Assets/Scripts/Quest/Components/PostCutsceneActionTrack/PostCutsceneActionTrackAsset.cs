using System;
using System.Linq;
using CryptoQuest.Quest.Actions;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace CryptoQuest.System.CutsceneSystem.CustomTimelineTracks.PostCutsceneActionTrack
{
    [Serializable]
    [TrackClipType(typeof(PostCutsceneActionPlayableAsset))]
    [TrackColor(238, 238, 238)]
    public class PostCutsceneActionTrackAsset : PlayableTrack
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            var clips = GetClips().ToList();
            for (int i = 0; i < clips.Count(); i++)
            {
                if (i > 0)
                    DeleteClip(clips[i]);
            }

            return base.CreateTrackMixer(graph, go, inputCount);
        }
    }
}