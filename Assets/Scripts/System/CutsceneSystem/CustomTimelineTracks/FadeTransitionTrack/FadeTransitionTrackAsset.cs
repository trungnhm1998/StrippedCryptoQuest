using System;
using UnityEngine.Timeline;

namespace CryptoQuest.System.CutsceneSystem.CustomTimelineTracks.FadeTransitionTrack
{
    [Serializable]
    [TrackClipType(typeof(FadeTransitionPlayableAsset))]
    [TrackColor(238, 238, 238)]
    public class FadeTransitionTrackAsset : PlayableTrack { }
}