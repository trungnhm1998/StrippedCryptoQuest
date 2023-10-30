using System;
using CryptoQuest.System.Dialogue.YarnManager;
using UnityEngine.Timeline;

namespace CryptoQuest.System.CutsceneSystem.CustomTimelineTracks.YarnSpinnerNodeControlTrack
{
    [Serializable]
    [TrackClipType(typeof(YarnSpinnerNodePlayableAsset))]
    [TrackBindingType(typeof(YarnProjectConfigSO))]
    [TrackColor(238, 238, 238)]
    public class YarnSpinnerNodeTrackAsset : PlayableTrack { }
}